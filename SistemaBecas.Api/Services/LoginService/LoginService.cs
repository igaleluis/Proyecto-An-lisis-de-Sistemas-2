using SistemaBecas.Api.Repositories.LoginRepository;
using SistemaBecas.Api.Services.JwtService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IJwtService _JwtService;
        public LoginService(ILoginRepository loginRepository, IJwtService JwtService)
        {
            _loginRepository = loginRepository;
            _JwtService = JwtService;
        }

        public async Task<LoginRespuestaDto> LoginAsync(LoginPeticiónDto peticion)
        {
            var usuario = await _loginRepository.ObtenerUsuarioPorCorreo(peticion.Correo);

            if (usuario == null)
            {
                return null;
            }
            if (usuario.Estado != "Activo")
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(
                    peticion.Password,
                    usuario.PasswordHash))
            {
                return null;
            }

            var token = _JwtService.GenerarToken(usuario);

            var usuarioDto = new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                Estado = usuario.Estado
            };

            return new LoginRespuestaDto
            {
                Token = token,
                Usuario = usuarioDto
            };
        }

        public async Task<UsuarioDto> ObtenerPorCorreoAsync(string correo)
        {
            var usuario = await _loginRepository.ObtenerUsuarioPorCorreo(correo);
            if (usuario == null)
            {
                return null;
            }
            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                Estado = usuario.Estado
            };
        }
    }
}
