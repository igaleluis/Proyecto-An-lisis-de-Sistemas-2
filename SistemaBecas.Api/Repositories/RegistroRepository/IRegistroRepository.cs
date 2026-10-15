using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.RegistroRepository
{
    public interface IRegistroRepository
    {
        Task<UsuarioDto?> RegistrarEstudiante(string nombre, string apellidos, string correo, string passwordHash);
    }
}
