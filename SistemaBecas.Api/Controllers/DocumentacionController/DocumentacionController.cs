using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.DocumentacionService;

namespace SistemaBecas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentacionController : ControllerBase
    {
        private readonly IDocumentacionService _service;

        public DocumentacionController(
            IDocumentacionService service)
        {
            _service = service;
        }

        [HttpGet("solicitudes/{idEstudiante}")]
        public async Task<IActionResult> ObtenerSolicitudesEstudiante(
            int idEstudiante)
        {
            var solicitudes =
                await _service.ObtenerSolicitudesEstudiante(idEstudiante);

            return Ok(solicitudes);
        }

        [HttpGet("solicitud/{idSolicitud}/documentos")]
        public async Task<IActionResult> ObtenerDocumentosSolicitud(
            int idSolicitud)
        {
            var documentos =
                await _service.ObtenerDocumentosSolicitud(idSolicitud);

            return Ok(documentos);
        }
    }
}