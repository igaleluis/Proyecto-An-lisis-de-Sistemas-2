
using Npgsql;
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

        private async Task<NpgsqlConnection> CrearConexion()
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }

        private static void AgregarFechas(
            NpgsqlCommand command,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            command.Parameters.AddWithValue(
                "p_fecha_inicio",
                NpgsqlTypes.NpgsqlDbType.Date,
                fechaInicio.HasValue
                    ? fechaInicio.Value.Date
                    : DBNull.Value);

            command.Parameters.AddWithValue(
                "p_fecha_fin",
                NpgsqlTypes.NpgsqlDbType.Date,
                fechaFin.HasValue
                    ? fechaFin.Value.Date
                    : DBNull.Value);
        }

        public async Task<List<SolicitudesPorConvocatoriaDto>>
            ObtenerSolicitudesPorConvocatoria(
                DateTime? fechaInicio,
                DateTime? fechaFin)
        {
            var resultado = new List<SolicitudesPorConvocatoriaDto>();

            await using var connection = await CrearConexion();

            await using var command = new NpgsqlCommand(
                @"SELECT *
                  FROM sp_reporte_solicitudes_por_convocatoria(
                      @p_fecha_inicio,
                      @p_fecha_fin
                  );",
                connection);

            AgregarFechas(command, fechaInicio, fechaFin);

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
            ObtenerEstadoSolicitudes(
                DateTime? fechaInicio,
                DateTime? fechaFin)
        {
            var resultado = new List<EstadoSolicitudesDto>();

            await using var connection = await CrearConexion();

            await using var command = new NpgsqlCommand(
                @"SELECT *
                  FROM sp_reporte_estado_solicitudes(
                      @p_fecha_inicio,
                      @p_fecha_fin
                  );",
                connection);

            AgregarFechas(command, fechaInicio, fechaFin);

            await using var reader = await command.ExecuteReaderAsync();

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

        public async Task<List<BecasOtorgadasDto>>
            ObtenerBecasOtorgadas(
                DateTime? fechaInicio,
                DateTime? fechaFin)
        {
            var resultado = new List<BecasOtorgadasDto>();

            await using var connection = await CrearConexion();

            await using var command = new NpgsqlCommand(
                @"SELECT *
                  FROM sp_reporte_becas_otorgadas(
                      @p_fecha_inicio,
                      @p_fecha_fin
                  );",
                connection);

            AgregarFechas(command, fechaInicio, fechaFin);

            await using var reader = await command.ExecuteReaderAsync();

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

        public async Task<List<ConvocatoriasReporteDto>>
            ObtenerConvocatorias(
                DateTime? fechaInicio,
                DateTime? fechaFin)
        {
            var resultado = new List<ConvocatoriasReporteDto>();

            await using var connection = await CrearConexion();

            await using var command = new NpgsqlCommand(
                @"SELECT *
                  FROM sp_reporte_convocatorias(
                      @p_fecha_inicio,
                      @p_fecha_fin
                  );",
                connection);

            AgregarFechas(command, fechaInicio, fechaFin);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                resultado.Add(new ConvocatoriasReporteDto
                {
                    IdConvocatoria = reader.GetInt32(
                        reader.GetOrdinal("idconvocatoria")),

                    Nombre = reader.GetString(
                        reader.GetOrdinal("nombre")),

                    Descripcion = reader.IsDBNull(
                        reader.GetOrdinal("descripcion"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("descripcion")),

                    FechaInicio = reader.GetDateTime(
                        reader.GetOrdinal("fechainicio")),

                    FechaFin = reader.GetDateTime(
                        reader.GetOrdinal("fechafin")),

                    Cupos = reader.GetInt32(
                        reader.GetOrdinal("cupos")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("estado")),

                    FechaCreacion = reader.GetDateTime(
                        reader.GetOrdinal("fechacreacion"))
                });
            }

            return resultado;
        }

        public async Task<EstadisticasReportesDto> ObtenerEstadisticas(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            await using var connection = await CrearConexion();

            await using var command = new NpgsqlCommand(
                @"SELECT *
                  FROM sp_reporte_estadisticas(
                      @p_fecha_inicio,
                      @p_fecha_fin
                  );",
                connection);

            AgregarFechas(command, fechaInicio, fechaFin);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new EstadisticasReportesDto
                {
                    TotalSolicitudes = reader.GetInt64(
                        reader.GetOrdinal("total_solicitudes")),

                    Aprobadas = reader.GetInt64(
                        reader.GetOrdinal("aprobadas")),

                    Rechazadas = reader.GetInt64(
                        reader.GetOrdinal("rechazadas")),

                    BecasOtorgadas = reader.GetInt64(
                        reader.GetOrdinal("becas_otorgadas"))
                };
            }

            return new EstadisticasReportesDto();
        }
    }
}
