using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.LoginRepository
{
    public interface ILoginRepository
    {
        Task<UsuarioLoginDataDto?> ObtenerUsuarioPorCorreo(string correo);
    }
}
