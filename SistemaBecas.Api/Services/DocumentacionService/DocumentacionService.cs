using SistemaBecas.Api.Repositories.DocumentacionRepository;
using SistemaBecas.Api.Services.DocumentacionService;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.Documentacion
{
    public class DocumentacionService : IDocumentacionService
    {
        private readonly IDocumentacionRepository _repository;

        public DocumentacionService(
            IDocumentacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante)
        {
            return await _repository
                .ObtenerSolicitudesEstudiante(idEstudiante);
        }

        public async Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud)
        {
            return await _repository
                .ObtenerDocumentosSolicitud(idSolicitud);
        }
    }
}