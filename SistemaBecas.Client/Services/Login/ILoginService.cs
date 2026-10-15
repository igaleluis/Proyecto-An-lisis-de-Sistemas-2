using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.Login
{
    public interface ILoginService
    {
        Task<LoginRespuestaDto> LoginAsync(LoginPeticiónDto peticion);
    }
}
