using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.RegistroService
{
    public interface IRegistroService
    {
        Task<UsuarioDto?> RegistrarEstudiante(RegistroEstudianteDto registro);
    }
}
