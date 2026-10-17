using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.EstadoBecaService;

namespace SistemaBecas.Api.Controllers.EstadoBecaController
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoBecaController : ControllerBase
    {
        private readonly IEstadoBecaService _estadoBecaService;

        public EstadoBecaController(
            IEstadoBecaService estadoBecaService)
        {
            _estadoBecaService = estadoBecaService;
        }

        [HttpGet("{idSolicitud}")]
        public async Task<IActionResult> ObtenerEstadoBeca(
            int idSolicitud)
        {
            var resultado = await _estadoBecaService
                .ObtenerEstadoBecaAsync(idSolicitud);

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensaje = "Solicitud no encontrada"
                });
            }

            return Ok(resultado);
        }
    }
}