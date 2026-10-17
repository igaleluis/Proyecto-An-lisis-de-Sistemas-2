using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SistemaBecas.Api.Repositories;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.GestionController;

[ApiController]
[Route("api")]
public class GestionController : ControllerBase
{
    private readonly DbConnectionBecas _connectionFactory;

    public GestionController(DbConnectionBecas connectionFactory) => _connectionFactory = connectionFactory;

    [HttpGet("convocatorias")]
    public async Task<ActionResult<IEnumerable<ConvocatoriaDto>>> ObtenerConvocatorias(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT idconvocatoria, nombre, COALESCE(descripcion, '') AS descripcion,
                   fechainicio, fechafin, COALESCE(cupos, 0) AS cupos, COALESCE(estado, '') AS estado
            FROM public.convocatoria
            ORDER BY idconvocatoria DESC;
            """;
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var items = new List<ConvocatoriaDto>();
        while (await reader.ReadAsync(cancellationToken)) items.Add(LeerConvocatoria(reader));
        return Ok(items);
    }

    [HttpPost("convocatorias")]
    public async Task<ActionResult<ConvocatoriaDto>> CrearConvocatoria(ConvocatoriaGuardarDto dto, CancellationToken cancellationToken)
    {
        if (!ValidarConvocatoria(dto, out var error)) return BadRequest(new { mensaje = error });
        const string sql = """
            INSERT INTO public.convocatoria (nombre, descripcion, fechainicio, fechafin, cupos, estado)
            VALUES (@nombre, @descripcion, @fechainicio, @fechafin, @cupos, @estado)
            RETURNING idconvocatoria, nombre, COALESCE(descripcion, '') AS descripcion,
                      fechainicio, fechafin, COALESCE(cupos, 0) AS cupos, COALESCE(estado, '') AS estado;
            """;
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        AgregarConvocatoria(command, dto);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var created = await reader.ReadAsync(cancellationToken) ? LeerConvocatoria(reader) : null;
        return created is null ? Problem("No se pudo crear la convocatoria.") : CreatedAtAction(nameof(ObtenerConvocatoria), new { id = created.IdConvocatoria }, created);
    }

    [HttpGet("convocatorias/{id:int}")]
    public async Task<ActionResult<ConvocatoriaDto>> ObtenerConvocatoria(int id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT idconvocatoria, nombre, COALESCE(descripcion, '') AS descripcion,
                   fechainicio, fechafin, COALESCE(cupos, 0) AS cupos, COALESCE(estado, '') AS estado
            FROM public.convocatoria WHERE idconvocatoria = @id;
            """;
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", id);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Ok(LeerConvocatoria(reader)) : NotFound();
    }

    [HttpPut("convocatorias/{id:int}")]
    public async Task<IActionResult> ActualizarConvocatoria(int id, ConvocatoriaGuardarDto dto, CancellationToken cancellationToken)
    {
        if (!ValidarConvocatoria(dto, out var error)) return BadRequest(new { mensaje = error });
        const string sql = """
            UPDATE public.convocatoria SET nombre=@nombre, descripcion=@descripcion,
                fechainicio=@fechainicio, fechafin=@fechafin, cupos=@cupos, estado=@estado
            WHERE idconvocatoria=@id;
            """;
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        AgregarConvocatoria(command, dto);
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken) == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("convocatorias/{id:int}")]
    public async Task<IActionResult> EliminarConvocatoria(int id, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand("DELETE FROM public.convocatoria WHERE idconvocatoria=@id", connection);
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken) == 0 ? NotFound() : NoContent();
    }

    [HttpGet("solicitudes")]
    public async Task<ActionResult<IEnumerable<SolicitudGestionDto>>> ObtenerSolicitudes(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT s.idsolicitud, s.idconvocatoria, s.idestudiante,
                   concat_ws(' ', u.nombres, u.apellidos) AS estudiante,
                   COALESCE(e.dpi, '') AS dpi, COALESCE(u.correo, '') AS correo,
                   COALESCE(e.telefono, '') AS telefono, COALESCE(e.direccion, '') AS direccion,
                   c.nombre AS convocatoria, s.fechasolicitud, COALESCE(s.estado, '') AS estado,
                   COALESCE(s.observaciones, '') AS observaciones, ev.punteo
            FROM public.solicitud s
            JOIN public.convocatoria c ON c.idconvocatoria=s.idconvocatoria
            LEFT JOIN public.estudiante e ON e.idestudiante=s.idestudiante
            LEFT JOIN public.usuario u ON u.idusuario=e.idusuario
            LEFT JOIN LATERAL (
                SELECT AVG(punteo)::numeric AS punteo FROM public.evaluacion WHERE idsolicitud=s.idsolicitud
            ) ev ON true
            ORDER BY s.fechasolicitud DESC, s.idsolicitud DESC;
            """;
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var items = new List<SolicitudGestionDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            items.Add(new SolicitudGestionDto
            {
                IdSolicitud = reader.GetInt32(reader.GetOrdinal("idsolicitud")),
                IdConvocatoria = reader.GetInt32(reader.GetOrdinal("idconvocatoria")),
                IdEstudiante = reader.GetInt32(reader.GetOrdinal("idestudiante")),
                Estudiante = reader.GetString(reader.GetOrdinal("estudiante")),
                Dpi = reader.GetString(reader.GetOrdinal("dpi")),
                Correo = reader.GetString(reader.GetOrdinal("correo")),
                Telefono = reader.GetString(reader.GetOrdinal("telefono")),
                Direccion = reader.GetString(reader.GetOrdinal("direccion")),
                Convocatoria = reader.GetString(reader.GetOrdinal("convocatoria")),
                FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("fechasolicitud")),
                Estado = reader.GetString(reader.GetOrdinal("estado")),
                Observaciones = reader.GetString(reader.GetOrdinal("observaciones")),
                Punteo = reader.IsDBNull(reader.GetOrdinal("punteo")) ? null : reader.GetDecimal(reader.GetOrdinal("punteo"))
            });
        }
        return Ok(items);
    }

    [HttpPut("solicitudes/{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstadoSolicitud(int id, SolicitudEstadoDto dto, CancellationToken cancellationToken)
    {
        var estados = new[] { "Pendiente", "En revisión", "En evaluación", "Aprobada", "Rechazada" };
        if (!estados.Contains(dto.Estado, StringComparer.OrdinalIgnoreCase)) return BadRequest(new { mensaje = "El estado indicado no es válido." });
        const string sql = "UPDATE public.solicitud SET estado=@estado, observaciones=@observaciones WHERE idsolicitud=@id";
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("estado", dto.Estado);
        command.Parameters.AddWithValue("observaciones", dto.Observaciones ?? "");
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken) == 0 ? NotFound() : NoContent();
    }

    private static ConvocatoriaDto LeerConvocatoria(NpgsqlDataReader reader) => new()
    {
        IdConvocatoria = reader.GetInt32(reader.GetOrdinal("idconvocatoria")),
        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
        Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
        FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechainicio")),
        FechaFin = reader.GetDateTime(reader.GetOrdinal("fechafin")),
        Cupos = reader.GetInt32(reader.GetOrdinal("cupos")),
        Estado = reader.GetString(reader.GetOrdinal("estado"))
    };

    private static void AgregarConvocatoria(NpgsqlCommand command, ConvocatoriaGuardarDto dto)
    {
        command.Parameters.AddWithValue("nombre", dto.Nombre.Trim()); command.Parameters.AddWithValue("descripcion", dto.Descripcion ?? "");
        command.Parameters.AddWithValue("fechainicio", dto.FechaInicio); command.Parameters.AddWithValue("fechafin", dto.FechaFin);
        command.Parameters.AddWithValue("cupos", dto.Cupos); command.Parameters.AddWithValue("estado", dto.Estado.Trim());
    }

    private static bool ValidarConvocatoria(ConvocatoriaGuardarDto dto, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(dto.Nombre)) error = "El nombre es obligatorio.";
        else if (dto.FechaFin < dto.FechaInicio) error = "La fecha de fin debe ser posterior a la de inicio.";
        else if (dto.Cupos < 0) error = "Los cupos no pueden ser negativos.";
        else if (!new[] { "Activa", "Próxima", "Cerrada" }.Contains(dto.Estado, StringComparer.OrdinalIgnoreCase)) error = "El estado indicado no es válido.";
        return error.Length == 0;
    }
}
