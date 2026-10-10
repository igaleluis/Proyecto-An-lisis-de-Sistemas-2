using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.EvaluacionService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.EvaluacionController
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionController : ControllerBase
    {
        private readonly IEvaluacionService _evaluacionService;

        public EvaluacionController(IEvaluacionService evaluacionService)
        {
            _evaluacionService = evaluacionService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEvaluaciones()
        {
            var evaluaciones = await _evaluacionService.ObtenerEvaluacionesAsync();

            return Ok(evaluaciones);
        }

        [HttpGet("{idEvaluacion}")]
        public async Task<IActionResult> ObtenerEvaluacionPorId(int idEvaluacion)
        {
            var evaluacion =
                await _evaluacionService.ObtenerEvaluacionPorIdAsync(idEvaluacion);

            if (evaluacion == null)
            {
                return NotFound(new
                {
                    mensaje = "Evaluación no encontrada"
                });
            }

            return Ok(evaluacion);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEvaluacion(EvaluacionDto evaluacion)
        {
            var idEvaluacion =
                await _evaluacionService.CrearEvaluacionAsync(evaluacion);

            return Ok(new
            {
                mensaje = "Evaluación registrada correctamente",
                idEvaluacion = idEvaluacion
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEvaluacion(EvaluacionDto evaluacion)
        {
            var actualizado =
                await _evaluacionService.ActualizarEvaluacionAsync(evaluacion);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Evaluación no encontrada"
                });
            }

            return Ok(new
            {
                mensaje = "Evaluación actualizada correctamente"
            });
        }
    }
}