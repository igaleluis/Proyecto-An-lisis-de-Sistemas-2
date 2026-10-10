using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.Email;

namespace SistemaBecas.Api.Controllers.EmailController
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("prueba")]
        public async Task<IActionResult> EnviarCorreoPrueba(
            string correo)
        {
            try
            {
                await _emailService.EnviarCorreoAsync(
                    correo,
                    "Prueba - Sistema de Becas",
                    """
                    <h2>Prueba de correo</h2>
                    <p>Este correo fue enviado desde el Sistema de Becas.</p>
                    <p>El servicio de correo está funcionando correctamente.</p>
                    """);

                return Ok(new
                {
                    mensaje = "Correo enviado correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "No fue posible enviar el correo.",
                    error = ex.Message
                });
            }
        }
    }
}
