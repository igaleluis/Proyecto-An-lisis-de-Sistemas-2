using SistemaBecas.Api.Repositories.RegistroRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.RegistroService
{
    public class RegistroService:IRegistroService
    {
        private readonly IRegistroRepository _registroRepository;
        public RegistroService(IRegistroRepository registroRepository)
        {
            _registroRepository = registroRepository;
        }

        public async Task<UsuarioDto?> RegistrarEstudiante(RegistroEstudianteDto registro)
        {
            // Generar hash de la contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(
                registro.Password);

            // Registrar estudiante
            return await _registroRepository.RegistrarEstudiante(
                registro.Nombres,
                registro.Apellidos,
                registro.Correo,
                passwordHash);
        }
    }
}
