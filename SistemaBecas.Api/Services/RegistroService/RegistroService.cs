
using SistemaBecas.Api.Repositories.RegistroRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.RegistroService
{
    public class RegistroService : IRegistroService
    {
        private readonly IRegistroRepository _registroRepository;

        public RegistroService(IRegistroRepository registroRepository)
        {
            _registroRepository = registroRepository;
        }

        public async Task<UsuarioDto?> RegistrarEstudiante(
            RegistroEstudianteDto registro)
        {
            // Generar el hash de la contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(
                registro.Password);

            // Validar que se haya ingresado la fecha de nacimiento
            if (!registro.FechaNacimiento.HasValue)
            {
                throw new ArgumentException(
                    "La fecha de nacimiento es obligatoria.");
            }

            // Registrar usuario y datos del estudiante
            return await _registroRepository.RegistrarEstudiante(
                registro.Nombres,
                registro.Apellidos,
                registro.Correo,
                passwordHash,
                registro.DPI,
                registro.FechaNacimiento.Value,
                registro.Telefono,
                registro.Direccion);
        }
    }
}
