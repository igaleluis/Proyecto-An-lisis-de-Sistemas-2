using Npgsql;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.ComiteRepository
{
    public class ComiteRepository : IComiteRepository
    {
        private readonly DbConnectionBecas _connection;

        public ComiteRepository(DbConnectionBecas connection)
        {
            _connection = connection;
        }

        public async Task<List<ComiteDto>> ObtenerComitesAsync()
        {
            const string sql = """
                SELECT 
                    c.idcomite,
                    c.nombre,
                    c.fecha,
                    c.descripcion,
                    c.estado,
                    COALESCE(
                        ARRAY_AGG(ce.idevaluador)
                        FILTER (WHERE ce.idevaluador IS NOT NULL),
                        ARRAY[]::integer[]
                    ) AS idevaluadores
                FROM public.comite c
                LEFT JOIN public.comiteevaluador ce
                    ON c.idcomite = ce.idcomite
                GROUP BY
                    c.idcomite,
                    c.nombre,
                    c.fecha,
                    c.descripcion,
                    c.estado
                ORDER BY c.idcomite;
                """;

            var comites = new List<ComiteDto>();

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                comites.Add(new ComiteDto
                {
                    IdComite = reader.GetInt32(
                        reader.GetOrdinal("idcomite")),

                    Nombre = reader.GetString(
                        reader.GetOrdinal("nombre")),

                    Fecha = reader.GetDateTime(
                        reader.GetOrdinal("fecha")),

                    Descripcion = reader.IsDBNull(
                        reader.GetOrdinal("descripcion"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("descripcion")),

                    Estado = reader.IsDBNull(
                        reader.GetOrdinal("estado"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("estado")),

                    IdEvaluadores = reader.GetFieldValue<int[]>(
                        reader.GetOrdinal("idevaluadores")).ToList()
                });
            }

            return comites;
        }

        public async Task<ComiteDto?> ObtenerComitePorIdAsync(int idComite)
        {
            const string sql = """
                SELECT 
                    c.idcomite,
                    c.nombre,
                    c.fecha,
                    c.descripcion,
                    c.estado,
                    COALESCE(
                        ARRAY_AGG(ce.idevaluador)
                        FILTER (WHERE ce.idevaluador IS NOT NULL),
                        ARRAY[]::integer[]
                    ) AS idevaluadores
                FROM public.comite c
                LEFT JOIN public.comiteevaluador ce
                    ON c.idcomite = ce.idcomite
                WHERE c.idcomite = @idcomite
                GROUP BY
                    c.idcomite,
                    c.nombre,
                    c.fecha,
                    c.descripcion,
                    c.estado;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("idcomite", idComite);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new ComiteDto
            {
                IdComite = reader.GetInt32(
                    reader.GetOrdinal("idcomite")),

                Nombre = reader.GetString(
                    reader.GetOrdinal("nombre")),

                Fecha = reader.GetDateTime(
                    reader.GetOrdinal("fecha")),

                Descripcion = reader.IsDBNull(
                    reader.GetOrdinal("descripcion"))
                    ? ""
                    : reader.GetString(
                        reader.GetOrdinal("descripcion")),

                Estado = reader.IsDBNull(
                    reader.GetOrdinal("estado"))
                    ? ""
                    : reader.GetString(
                        reader.GetOrdinal("estado")),

                IdEvaluadores = reader.GetFieldValue<int[]>(
                    reader.GetOrdinal("idevaluadores")).ToList()
            };
        }

        public async Task<int> CrearComiteAsync(ComiteDto comite)
        {
            const string sql = """
                INSERT INTO public.comite
                    (nombre, fecha, descripcion, estado)
                VALUES
                    (@nombre, @fecha, @descripcion, @estado)
                RETURNING idcomite;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("nombre", comite.Nombre);
            command.Parameters.AddWithValue("fecha", comite.Fecha.Date);
            command.Parameters.AddWithValue("descripcion", comite.Descripcion);
            command.Parameters.AddWithValue("estado", comite.Estado);

            var resultado = await command.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }

        public async Task<bool> ActualizarComiteAsync(ComiteDto comite)
        {
            const string sql = """
                UPDATE public.comite
                SET
                    nombre = @nombre,
                    fecha = @fecha,
                    descripcion = @descripcion,
                    estado = @estado
                WHERE idcomite = @idcomite;
                """;

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("idcomite", comite.IdComite);
            command.Parameters.AddWithValue("nombre", comite.Nombre);
            command.Parameters.AddWithValue("fecha", comite.Fecha.Date);
            command.Parameters.AddWithValue("descripcion", comite.Descripcion);
            command.Parameters.AddWithValue("estado", comite.Estado);

            var filasAfectadas = await command.ExecuteNonQueryAsync();

            return filasAfectadas > 0;
        }

        public async Task<bool> AsignarEvaluadoresAsync(
            int idComite,
            List<int> idEvaluadores)
        {
            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var transaction =
                await connection.BeginTransactionAsync();

            try
            {
                const string deleteSql = """
                    DELETE FROM public.comiteevaluador
                    WHERE idcomite = @idcomite;
                    """;

                await using var deleteCommand =
                    new NpgsqlCommand(deleteSql, connection, transaction);

                deleteCommand.Parameters.AddWithValue(
                    "idcomite",
                    idComite);

                await deleteCommand.ExecuteNonQueryAsync();

                const string insertSql = """
                    INSERT INTO public.comiteevaluador
                        (idcomite, idevaluador)
                    VALUES
                        (@idcomite, @idevaluador);
                    """;

                foreach (var idEvaluador in idEvaluadores.Distinct())
                {
                    await using var insertCommand =
                        new NpgsqlCommand(
                            insertSql,
                            connection,
                            transaction);

                    insertCommand.Parameters.AddWithValue(
                        "idcomite",
                        idComite);

                    insertCommand.Parameters.AddWithValue(
                        "idevaluador",
                        idEvaluador);

                    await insertCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<EvaluadorDto>> ObtenerEvaluadoresAsync()
        {
            const string sql = """
                SELECT
                    e.idevaluador,
                    e.idusuario,
                    u.nombres || ' ' || u.apellidos AS nombre,
                    e.cargo,
                    e.estado
                FROM public.evaluador e
                INNER JOIN public.usuario u
                    ON e.idusuario = u.idusuario
                WHERE e.estado = 'Activo'
                ORDER BY u.nombres, u.apellidos;
                """;

            var evaluadores = new List<EvaluadorDto>();

            await using var connection = _connection.CreateConnection();
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                evaluadores.Add(new EvaluadorDto
                {
                    IdEvaluador = reader.GetInt32(
                        reader.GetOrdinal("idevaluador")),

                    IdUsuario = reader.GetInt32(
                        reader.GetOrdinal("idusuario")),

                    Nombre = reader.GetString(
                        reader.GetOrdinal("nombre")),

                    Cargo = reader.GetString(
                        reader.GetOrdinal("cargo")),

                    Estado = reader.GetString(
                        reader.GetOrdinal("estado"))
                });
            }

            return evaluadores;
        }
    }
}