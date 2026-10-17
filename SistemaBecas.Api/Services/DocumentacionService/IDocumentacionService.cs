using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.Documentacion
{
    public interface IDocumentacionService
    {
        Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante);

        Task<List<SolicitudDocumentacionDto>>
            ObtenerMisSolicitudes(int idUsuario);

        Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud);
        Task<bool> SolicitudPerteneceAUsuario(
            int idSolicitud,
            int idUsuario);
        Task<int> GuardarDocumentoSolicitud(
            int idSolicitud,
            int idDocumento,
            string rutaArchivo,
            string? observaciones);

        Task<int> SubirDocumento(
            int idSolicitud,
            int idDocumento,
            Stream archivo,
            string nombreArchivo,
            string contentType,
            string? observaciones,
            int idUsuario);
    }
}