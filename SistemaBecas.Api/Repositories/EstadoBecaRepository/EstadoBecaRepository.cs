using Npgsql;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.EstadoBecaRepository
{
    public class EstadoBecaRepository : IEstadoBecaRepository
    {
        private readonly DbConnectionBecas _connection;

        public EstadoBecaRepository(DbConnectionBecas connection)
        {
            _connection = connection;
        }

        public async Task<EstadoBecaDto?> ObtenerEstadoBecaAsync(int idSolicitud)
        {
            const string sql = """
                SELECT
                    s.idsolicitud,
                    CONCAT(u.nombres, ' ', u.apellidos) AS estudiante,
                    c.nombre AS convocatoria,
                    s.fechasolicitud,
                    s.estado AS estado_solicitud,

                    h.estadoanterior,
                    h.estadonuevo,
                    h.fechacambio,

                    b.estado AS estado_beca,
                    b.monto,
                    b.fechainicio,
                    b.fechafin,
                    b.observaciones AS observaciones_beca

                FROM public.solicitud s

                INNER JOIN public.estudiante e
                    ON e.idestudiante = s.idestudiante

                INNER JOIN public.usuario u
                    ON u.idusuario = e.idusuario

                INNER JOIN public.convocatoria c
                    ON c.idconvocatoria = s.idconvocatoria

                LEFT JOIN public.historialsolicitud h
                    ON h.idsolicitud = s.idsolicitud

                LEFT JOIN public.beca b
                    ON b.idsolicitud = s.idsolicitud

                WHERE s.idsolicitud = @idSolicitud

                ORDER BY h.fechacambio;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("idSolicitud", idSolicitud);

            await using var reader = await command.ExecuteReaderAsync();

            EstadoBecaDto? resultado = null;

            while (await reader.ReadAsync())
            {
                if (resultado == null)
                {
                    resultado = new EstadoBecaDto
                    {
                        IdSolicitud = reader.GetInt32(
                            reader.GetOrdinal("idsolicitud")),

                        Estudiante = reader.GetString(
                            reader.GetOrdinal("estudiante")),

                        Convocatoria = reader.GetString(
                            reader.GetOrdinal("convocatoria")),

                        FechaSolicitud = reader.GetDateTime(
                            reader.GetOrdinal("fechasolicitud")),

                        EstadoSolicitud = reader.GetString(
                            reader.GetOrdinal("estado_solicitud"))
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("estado_beca")))
                    {
                        resultado.Beca = new BecaEstadoDto
                        {
                            Estado = reader.GetString(
                                reader.GetOrdinal("estado_beca")),

                            Monto = reader.IsDBNull(
                                reader.GetOrdinal("monto"))
                                ? null
                                : reader.GetDecimal(
                                    reader.GetOrdinal("monto")),

                            FechaInicio = reader.IsDBNull(
                                reader.GetOrdinal("fechainicio"))
                                ? null
                                : reader.GetDateTime(
                                    reader.GetOrdinal("fechainicio")),

                            FechaFin = reader.IsDBNull(
                                reader.GetOrdinal("fechafin"))
                                ? null
                                : reader.GetDateTime(
                                    reader.GetOrdinal("fechafin")),

                            Observaciones = reader.IsDBNull(
                                reader.GetOrdinal("observaciones_beca"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("observaciones_beca"))
                        };
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("estadonuevo")))
                {
                    resultado.Historial.Add(new HistorialEstadoDto
                    {
                        EstadoAnterior = reader.IsDBNull(
                            reader.GetOrdinal("estadoanterior"))
                            ? ""
                            : reader.GetString(
                                reader.GetOrdinal("estadoanterior")),

                        EstadoNuevo = reader.GetString(
                            reader.GetOrdinal("estadonuevo")),

                        FechaCambio = reader.GetDateTime(
                            reader.GetOrdinal("fechacambio"))
                    });
                }
            }

            return resultado;
        }
    }
}