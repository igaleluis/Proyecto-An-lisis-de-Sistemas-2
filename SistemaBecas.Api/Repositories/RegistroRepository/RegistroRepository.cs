using Npgsql;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.RegistroRepository
{
    public class RegistroRepository:IRegistroRepository
    {
        private readonly IConfiguration _configuration;
        public RegistroRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UsuarioDto?> RegistrarEstudiante(string nombre, string apellidos, string correo, string passwordHash)
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM public.registrar_estudiante(@p_nombres, @p_apellidos, @p_correo, @p_passwordhash)",
                connection);

            command.Parameters.AddWithValue(
                "p_nombres",
                nombre);

            command.Parameters.AddWithValue(
                "p_apellidos",
                apellidos);

            command.Parameters.AddWithValue(
                "p_correo",
                correo);

            command.Parameters.AddWithValue(
                "p_passwordhash",
                passwordHash);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new UsuarioDto
            {
                IdUsuario = reader.GetInt32(
                    reader.GetOrdinal("idusuario")),

                Nombres = reader.GetString(
                    reader.GetOrdinal("nombres")),

                Apellidos = reader.GetString(
                    reader.GetOrdinal("apellidos")),

                Correo = reader.GetString(
                    reader.GetOrdinal("correo")),

                Rol = reader.GetString(
                    reader.GetOrdinal("rol")),

                Estado = reader.GetString(
                    reader.GetOrdinal("estado"))
            };
        }
    }
}
