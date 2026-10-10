using Npgsql;
using SistemaBecas.Api.Repositories.Reportes;
using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Repositories.Reportes
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly IConfiguration _configuration;

        public ReportesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<SolicitudesPorConvocatoriaDto>> ObtenerSolicitudesPorConvocatoria()
        {
            var resultado = new List<SolicitudesPorConvocatoriaDto>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM sp_reporte_solicitudes_por_convocatoria();",
                connection);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new SolicitudesPorConvocatoriaDto
                {
                    IdConvocatoria = reader.GetInt32(
                        reader.GetOrdinal("idconvocatoria")),

                    Convocatoria = reader.GetString(
                        reader.GetOrdinal("convocatoria")),

                    TotalSolicitudes = reader.GetInt64(
                        reader.GetOrdinal("total_solicitudes")),

                    EnEvaluacion = reader.GetInt64(
                        reader.GetOrdinal("en_evaluacion")),

                    Aprobadas = reader.GetInt64(
                        reader.GetOrdinal("aprobadas")),

                    Rechazadas = reader.GetInt64(
                        reader.GetOrdinal("rechazadas"))
                });
            }

            return resultado;
        }
        public async Task<List<EstadoSolicitudesDto>>
        ObtenerEstadoSolicitudes()
        {
            var resultado = new List<EstadoSolicitudesDto>();

            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM sp_reporte_estado_solicitudes();",
                connection);

            await using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new EstadoSolicitudesDto
                {
                    IdSolicitud = reader.GetInt32(
                        reader.GetOrdinal("idsolicitud")),

                    Estudiante = reader.GetString(
                        reader.GetOrdinal("estudiante")),

                    Convocatoria = reader.GetString(
                        reader.GetOrdinal("convocatoria")),

                    FechaSolicitud = reader.GetDateTime(
                        reader.GetOrdinal("fechasolicitud")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("estado")),

                    Observaciones = reader.IsDBNull(
                        reader.GetOrdinal("observaciones"))
                        ? string.Empty
                        : reader.GetString(
                            reader.GetOrdinal("observaciones"))
                });
            }

            return resultado;
        }

        public async Task<List<BecasOtorgadasDto>> ObtenerBecasOtorgadas()
        {
            var resultado = new List<BecasOtorgadasDto>();

            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM sp_reporte_becas_otorgadas();",
                connection);

            await using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new BecasOtorgadasDto
                {
                    IdConvocatoria = reader.GetInt32(
                        reader.GetOrdinal("idconvocatoria")),

                    Convocatoria = reader.GetString(
                        reader.GetOrdinal("convocatoria")),

                    EstadoConvocatoria = reader.GetString(
                        reader.GetOrdinal("estado_convocatoria")),

                    Cupos = reader.GetInt32(
                        reader.GetOrdinal("cupos")),

                    BecasOtorgadas = reader.GetInt64(
                        reader.GetOrdinal("becas_otorgadas")),

                    CuposRestantes = reader.GetInt64(
                        reader.GetOrdinal("cupos_restantes")),

                    IdSolicitud = reader.IsDBNull(
                        reader.GetOrdinal("idsolicitud"))
                        ? null
                        : reader.GetInt32(
                            reader.GetOrdinal("idsolicitud")),

                    Estudiante = reader.IsDBNull(
                        reader.GetOrdinal("estudiante"))
                        ? string.Empty
                        : reader.GetString(
                            reader.GetOrdinal("estudiante")),

                    FechaSolicitud = reader.IsDBNull(
                        reader.GetOrdinal("fechasolicitud"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("fechasolicitud")),

                    EstadoSolicitud = reader.IsDBNull(
                        reader.GetOrdinal("estado_solicitud"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("estado_solicitud"))
                });
            }

            return resultado;
        }

    }
}