using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SistemaBecas.Api.Services.RegistroService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.RegistroController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroController : ControllerBase
    {
        private readonly IRegistroService _registroService;

        public RegistroController(IRegistroService registroService)
        {
            _registroService = registroService;
        }

        [HttpPost("RegistroEstudiante")]
        public async Task<IActionResult> RegistrarEstudiante(
            RegistroEstudianteDto registro)
        {
            try
            {
                var usuario = await _registroService.RegistrarEstudiante(registro);

                if (usuario == null)
                {
                    return BadRequest(new
                    {
                        mensaje = "No fue posible registrar al estudiante."
                    });
                }

                return Ok(usuario);
            }
            catch (PostgresException ex) when (ex.SqlState == "P0001")
            {
                return Conflict(new
                {
                    mensaje = ex.MessageText
                });
            }
        }
    }
}
