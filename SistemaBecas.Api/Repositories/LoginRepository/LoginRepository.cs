using SistemaBecas.Library.Dtos;
using Npgsql;

namespace SistemaBecas.Api.Repositories.LoginRepository
{
    public class LoginRepository:ILoginRepository
    {
        private readonly DbConnectionBecas _connection;

        public LoginRepository(DbConnectionBecas connection)
        {
            _connection = connection;
        }


        //Método para login 
        public async Task<UsuarioLoginDataDto?> ObtenerUsuarioPorCorreo(string correo)
        {
            const string sql = """
                SELECT *
                FROM public.sp_usuario_por_correo(@correo);
                """;
            await using var connection = _connection.CreateConnection();

            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("correo", correo);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }
            return new UsuarioLoginDataDto
            {
                IdUsuario = reader.GetInt32(
                reader.GetOrdinal("idusuario")),

                Nombres = reader.GetString(
                reader.GetOrdinal("nombres")),

                Apellidos = reader.GetString(
                reader.GetOrdinal("apellidos")),

                Correo = reader.GetString(
                reader.GetOrdinal("correo")),

                PasswordHash = reader.GetString(
                reader.GetOrdinal("passwordhash")),

                Rol = reader.GetString(
                reader.GetOrdinal("rol")),

                Estado = reader.GetString(
                reader.GetOrdinal("estado"))
            };
        }
    }
}
