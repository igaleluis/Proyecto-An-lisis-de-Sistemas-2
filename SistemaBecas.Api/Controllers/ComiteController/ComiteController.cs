using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.ComiteService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Controllers.ComiteController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComiteController : ControllerBase
    {
        private readonly IComiteService _comiteService;

        public ComiteController(IComiteService comiteService)
        {
            _comiteService = comiteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerComites()
        {
            var comites = await _comiteService.ObtenerComitesAsync();

            return Ok(comites);
        }

        [HttpGet("{idComite}")]
        public async Task<IActionResult> ObtenerComitePorId(int idComite)
        {
            var comite = await _comiteService.ObtenerComitePorIdAsync(idComite);

            if (comite == null)
            {
                return NotFound(new
                {
                    mensaje = "Comité no encontrado"
                });
            }

            return Ok(comite);
        }

        [HttpGet("evaluadores")]
        public async Task<IActionResult> ObtenerEvaluadores()
        {
            var evaluadores = await _comiteService.ObtenerEvaluadoresAsync();

            return Ok(evaluadores);
        }

        [HttpPost]
        public async Task<IActionResult> CrearComite(ComiteDto comite)
        {
            var idComite = await _comiteService.CrearComiteAsync(comite);

            return Ok(new
            {
                mensaje = "Comité registrado correctamente",
                idComite = idComite
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarComite(ComiteDto comite)
        {
            var actualizado =
                await _comiteService.ActualizarComiteAsync(comite);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "Comité no encontrado"
                });
            }

            return Ok(new
            {
                mensaje = "Comité actualizado correctamente"
            });
        }

        [HttpPut("{idComite}/evaluadores")]
        public async Task<IActionResult> AsignarEvaluadores(
            int idComite,
            List<int> idEvaluadores)
        {
            var resultado =
                await _comiteService.AsignarEvaluadoresAsync(
                    idComite,
                    idEvaluadores);

            if (!resultado)
            {
                return NotFound(new
                {
                    mensaje = "No se pudo asignar los evaluadores"
                });
            }

            return Ok(new
            {
                mensaje = "Evaluadores asignados correctamente"
            });
        }
    }
}