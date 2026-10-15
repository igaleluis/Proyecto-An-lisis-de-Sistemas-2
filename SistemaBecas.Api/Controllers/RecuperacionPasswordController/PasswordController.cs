using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.RecuperacionPassword;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.RecuperacionPasswordController
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IRecuperacionPasswordService _service;

        public PasswordController(
            IRecuperacionPasswordService service)
        {
            _service = service;
        }

        [HttpPost("solicitar")]
        public async Task<IActionResult> Solicitar(
            RecuperarContraseñaDto dto)
        {
            try
            {
                await _service.SolicitarRecuperacion(dto);

                return Ok(new
                {
                    mensaje = "Si el correo está registrado, recibirás un código de recuperación."
                });
            }
            catch
            {
                return Ok(new
                {
                    mensaje = "Si el correo está registrado, recibirás un código de recuperación."
                });
            }
        }

        [HttpPost("verificar")]
        public async Task<IActionResult> Verificar(
            VerificarCodigoDto dto)
        {
            var resultado =
                await _service.VerificarCodigo(dto);

            if (!resultado)
            {
                return BadRequest(new
                {
                    mensaje = "El código no es válido o ha expirado."
                });
            }

            return Ok(new
            {
                mensaje = "Código válido."
            });
        }

        [HttpPost("cambiar")]
        public async Task<IActionResult> Cambiar(
            CambiarPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NuevaPassword))
            {
                return BadRequest(new
                {
                    mensaje = "La nueva contraseña es obligatoria."
                });
            }

            if (dto.NuevaPassword.Length < 8)
            {
                return BadRequest(new
                {
                    mensaje = "La contraseña debe tener al menos 8 caracteres."
                });
            }

            var resultado =
                await _service.CambiarPassword(dto);

            if (!resultado)
            {
                return BadRequest(new
                {
                    mensaje = "El código no es válido o ha expirado."
                });
            }

            return Ok(new
            {
                mensaje = "La contraseña fue cambiada correctamente."
            });
        }
    }
}
