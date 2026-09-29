using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.LoginService
{
    public interface ILoginService
    {
        Task<UsuarioDto> ObtenerPorCorreoAsync(string correo);
        Task<LoginRespuestaDto> LoginAsync(LoginPeticiónDto peticion);
    }
}
