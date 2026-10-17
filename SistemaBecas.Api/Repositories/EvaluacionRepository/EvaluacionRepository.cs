using Npgsql;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.EvaluacionRepository
{
    public class EvaluacionRepository : IEvaluacionRepository
    {
        private readonly DbConnectionBecas _connection;

        public EvaluacionRepository(DbConnectionBecas connection)
        {
            _connection = connection;
        }

        public async Task<List<EvaluacionDto>> ObtenerEvaluacionesAsync()
        {
            const string sql = """
                SELECT
                    idevaluacion,
                    idsolicitud,
                    idevaluador,
                    fechaevaluacion,
                    punteo,
                    observaciones,
                    estado
                FROM public.evaluacion
                ORDER BY idevaluacion;
                """;

            var evaluaciones = new List<EvaluacionDto>();

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                evaluaciones.Add(new EvaluacionDto
                {
                    IdEvaluacion = reader.GetInt32(reader.GetOrdinal("idevaluacion")),
                    IdSolicitud = reader.GetInt32(reader.GetOrdinal("idsolicitud")),
                    IdEvaluador = reader.GetInt32(reader.GetOrdinal("idevaluador")),
                    FechaEvaluacion = reader.GetDateTime(reader.GetOrdinal("fechaevaluacion")),
                    Punteo = reader.IsDBNull(reader.GetOrdinal("punteo"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("punteo")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("observaciones")),
                    Estado = reader.IsDBNull(reader.GetOrdinal("estado"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("estado"))
                });
            }

            return evaluaciones;
        }

        public async Task<EvaluacionDto?> ObtenerEvaluacionPorIdAsync(int idEvaluacion)
        {
            const string sql = """
                SELECT
                    idevaluacion,
                    idsolicitud,
                    idevaluador,
                    fechaevaluacion,
                    punteo,
                    observaciones,
                    estado
                FROM public.evaluacion
                WHERE idevaluacion = @idEvaluacion;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("idEvaluacion", idEvaluacion);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new EvaluacionDto
                {
                    IdEvaluacion = reader.GetInt32(reader.GetOrdinal("idevaluacion")),
                    IdSolicitud = reader.GetInt32(reader.GetOrdinal("idsolicitud")),
                    IdEvaluador = reader.GetInt32(reader.GetOrdinal("idevaluador")),
                    FechaEvaluacion = reader.GetDateTime(reader.GetOrdinal("fechaevaluacion")),
                    Punteo = reader.IsDBNull(reader.GetOrdinal("punteo"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("punteo")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("observaciones"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("observaciones")),
                    Estado = reader.IsDBNull(reader.GetOrdinal("estado"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("estado"))
                };
            }

            return null;
        }

        public async Task<int> CrearEvaluacionAsync(EvaluacionDto evaluacion)
        {
            const string sql = """
                INSERT INTO public.evaluacion
                (
                    idsolicitud,
                    idevaluador,
                    fechaevaluacion,
                    punteo,
                    observaciones,
                    estado
                )
                VALUES
                (
                    @idSolicitud,
                    @idEvaluador,
                    @fechaEvaluacion,
                    @punteo,
                    @observaciones,
                    @estado
                )
                RETURNING idevaluacion;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("idSolicitud", evaluacion.IdSolicitud);
            command.Parameters.AddWithValue("idEvaluador", evaluacion.IdEvaluador);
            command.Parameters.AddWithValue("fechaEvaluacion", evaluacion.FechaEvaluacion);

            command.Parameters.AddWithValue(
                "punteo",
                evaluacion.Punteo.HasValue
                    ? evaluacion.Punteo.Value
                    : DBNull.Value);

            command.Parameters.AddWithValue(
                "observaciones",
                string.IsNullOrWhiteSpace(evaluacion.Observaciones)
                    ? DBNull.Value
                    : evaluacion.Observaciones);

            command.Parameters.AddWithValue(
                "estado",
                string.IsNullOrWhiteSpace(evaluacion.Estado)
                    ? DBNull.Value
                    : evaluacion.Estado);

            var resultado = await command.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }

        public async Task<bool> ActualizarEvaluacionAsync(EvaluacionDto evaluacion)
        {
            const string sql = """
                UPDATE public.evaluacion
                SET
                    idsolicitud = @idSolicitud,
                    idevaluador = @idEvaluador,
                    fechaevaluacion = @fechaEvaluacion,
                    punteo = @punteo,
                    observaciones = @observaciones,
                    estado = @estado
                WHERE idevaluacion = @idEvaluacion;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("idEvaluacion", evaluacion.IdEvaluacion);
            command.Parameters.AddWithValue("idSolicitud", evaluacion.IdSolicitud);
            command.Parameters.AddWithValue("idEvaluador", evaluacion.IdEvaluador);
            command.Parameters.AddWithValue("fechaEvaluacion", evaluacion.FechaEvaluacion);

            command.Parameters.AddWithValue(
                "punteo",
                evaluacion.Punteo.HasValue
                    ? evaluacion.Punteo.Value
                    : DBNull.Value);

            command.Parameters.AddWithValue(
                "observaciones",
                string.IsNullOrWhiteSpace(evaluacion.Observaciones)
                    ? DBNull.Value
                    : evaluacion.Observaciones);

            command.Parameters.AddWithValue(
                "estado",
                string.IsNullOrWhiteSpace(evaluacion.Estado)
                    ? DBNull.Value
                    : evaluacion.Estado);

            var filasAfectadas = await command.ExecuteNonQueryAsync();

            return filasAfectadas > 0;
        }
    }
}