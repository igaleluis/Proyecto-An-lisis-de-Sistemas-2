using Npgsql;

namespace SistemaBecas.Api.Repositories.RecuperacionPassword
{
    public class RecuperacionPasswordRepository : IRecuperacionPasswordRepository
    {
        private readonly IConfiguration _configuration;
        public RecuperacionPasswordRepository(IConfiguration configuration)
        {
            _configuration = configuration;   
        }
        public async Task<int?> ObtenerIdUsuarioPorCorreo(
            string correo)
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command =
                new NpgsqlCommand(
                    "SELECT public.obtener_id_usuario_por_correo(@p_correo)",
                    connection);

            command.Parameters.AddWithValue(
                "p_correo",
                correo);

            var resultado =
                await command.ExecuteScalarAsync();

            if (resultado == null ||
                resultado == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(resultado);
        }

        public async Task<bool> GuardarCodigo(
    int idUsuario,
    string codigo)
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command =
                new NpgsqlCommand(
                    """
            SELECT public.guardar_codigo_recuperacion(
                @p_idusuario,
                @p_codigo
            )
            """,
                    connection);

            command.Parameters.AddWithValue(
                "p_idusuario",
                idUsuario);

            command.Parameters.AddWithValue(
                "p_codigo",
                codigo);

            var resultado =
                await command.ExecuteScalarAsync();

            return resultado != null &&
                   resultado != DBNull.Value &&
                   Convert.ToBoolean(resultado);
        }

        public async Task<bool> VerificarCodigo(
            string correo,
            string codigo)
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command =
                new NpgsqlCommand(
                    """
                    SELECT public.verificar_codigo_recuperacion(
                        @p_correo,
                        @p_codigo
                    )
                    """,
                    connection);

            command.Parameters.AddWithValue(
                "p_correo",
                correo);

            command.Parameters.AddWithValue(
                "p_codigo",
                codigo);

            var resultado =
                await command.ExecuteScalarAsync();

            return resultado != null &&
                   resultado != DBNull.Value &&
                   Convert.ToBoolean(resultado);
        }

        public async Task<bool> CambiarPassword(
            string correo,
            string codigo,
            string passwordHash)
        {
            var connectionString =
                _configuration.GetConnectionString(
                    "DefaultConnection");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command =
                new NpgsqlCommand(
                    """
                    SELECT public.cambiar_password(
                        @p_correo,
                        @p_codigo,
                        @p_passwordhash
                    )
                    """,
                    connection);

            command.Parameters.AddWithValue(
                "p_correo",
                correo);

            command.Parameters.AddWithValue(
                "p_codigo",
                codigo);

            command.Parameters.AddWithValue(
                "p_passwordhash",
                passwordHash);

            var resultado =
                await command.ExecuteScalarAsync();

            return resultado != null &&
                   resultado != DBNull.Value &&
                   Convert.ToBoolean(resultado);
        }
    }
}
