
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.DocumentacionService
{
    public interface IDocumentacionService
    {
        Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante);

        Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud);
    }
}
