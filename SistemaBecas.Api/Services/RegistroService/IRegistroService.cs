using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.RegistroService
{
    public interface IRegistroService
    {
        Task<UsuarioDto?> RegistrarEstudiante(
            RegistroEstudianteDto registro);
    }
}
