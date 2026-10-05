using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaBecas.Api.Services.Documentacion;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        [Authorize(Roles = "Estudiante")]
        [HttpGet("mis-solicitudes")]
        public async Task<IActionResult> ObtenerMisSolicitudes()
        {
            var idUsuarioClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return Unauthorized();
            }

            var solicitudes =
                await _service.ObtenerMisSolicitudes(idUsuario);

            return Ok(solicitudes);
        }

        [Authorize(Roles = "Estudiante")]
        [HttpGet("solicitud/{idSolicitud}/documentos")]
        public async Task<IActionResult> ObtenerDocumentosSolicitud(
    int idSolicitud)
        {
            var idUsuarioClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return Unauthorized();
            }

            var pertenece =
                await _service.SolicitudPerteneceAUsuario(
                    idSolicitud,
                    idUsuario);

            if (!pertenece)
            {
                return Forbid();
            }

            var documentos =
                await _service.ObtenerDocumentosSolicitud(
                    idSolicitud);

            return Ok(documentos);
        }
        [Authorize(Roles = "Estudiante")]
        [HttpPost("subir-documento")]
        public async Task<IActionResult> SubirDocumento(
            [FromForm] int idSolicitud,
            [FromForm] int idDocumento,
            [FromForm] IFormFile archivo,
            [FromForm] string? observaciones)
        {
            var idUsuarioClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return Unauthorized();
            }

            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("Debe seleccionar un archivo.");
            }

            var extensionesPermitidas = new[]
            {
                ".pdf",
                ".jpg",
                ".jpeg",
                ".png"
            };

            var extension =
                Path.GetExtension(archivo.FileName)
                    .ToLowerInvariant();

            if (!extensionesPermitidas.Contains(extension))
            {
                return BadRequest(
                    "Solo se permiten archivos PDF, JPG, JPEG o PNG.");
            }

            var tiposPermitidos = new[]
            {
                "application/pdf",
                "image/jpeg",
                "image/png"
            };

            if (!tiposPermitidos.Contains(archivo.ContentType))
            {
                return BadRequest(
                    "El tipo de archivo no es permitido.");
            }

            if (archivo.Length > 10 * 1024 * 1024)
            {
                return BadRequest(
                    "El archivo no puede superar los 10 MB.");
            }

            try
            {
                await using var stream =
                    archivo.OpenReadStream();

                var idSolicitudDocumento =
                    await _service.SubirDocumento(
                        idSolicitud,
                        idDocumento,
                        stream,
                        Path.GetFileName(archivo.FileName),
                        archivo.ContentType,
                        observaciones,
                        idUsuario);

                return Ok(new
                {
                    mensaje = "Documento cargado correctamente.",
                    idSolicitudDocumento
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        mensaje = "No fue posible cargar el documento.",
                        detalle = ex.Message
                    });
            }
        }
    }
}