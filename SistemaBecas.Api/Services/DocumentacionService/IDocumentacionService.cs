using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.DocumentacionService
{
    public interface IDocumentacionService
    {
        Task<List<SolicitudDocumentacionDto>>
           ObtenerSolicitudesEstudiante(int idEstudiante);

        Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud);
    }
}
