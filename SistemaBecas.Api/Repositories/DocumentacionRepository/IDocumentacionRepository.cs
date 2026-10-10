using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.DocumentacionRepository
{
    public interface IDocumentacionRepository
    {
        Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante);

        Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud);
        Task<int> GuardarDocumentoSolicitud(
            int idSolicitud,
            int idDocumento,
            string rutaArchivo,
            string? observaciones);
    }
}
