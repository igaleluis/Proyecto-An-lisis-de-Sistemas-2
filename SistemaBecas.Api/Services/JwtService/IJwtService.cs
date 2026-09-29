using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.JwtService
{
    public interface IJwtService
    {
        string GenerarToken(UsuarioLoginDataDto loginData);
    }
}
