using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.LoginService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.LoginController
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("correo")]
        public async Task<IActionResult> ObtenerPorCorreo(string correo)
        {
            var usuario = await _loginService.ObtenerPorCorreoAsync(correo);

            if (usuario == null)
            {
                return NotFound(new
                {
                    mensaje = "Usuario no encontrado"
                });
            }

            return Ok(usuario);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginPeticiónDto peticion)
        {
            var resultado = await _loginService.LoginAsync(peticion);
            if (resultado == null)
            {
                return Unauthorized(new
                {
                    mensaje = "Correo o contraseña incorrectos"
                });
            }
            return Ok(resultado);
        }
    }
}