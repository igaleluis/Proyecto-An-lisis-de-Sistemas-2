using SistemaBecas.Api.Repositories.DocumentacionRepository;
using SistemaBecas.Api.Repositories.EstudianteRepository;
using SistemaBecas.Api.Services.SupabaseStorage;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.Documentacion
{
    public class DocumentacionService : IDocumentacionService
    {
        private readonly IDocumentacionRepository _repository;
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly ISupabaseStorageService _supabaseStorageService;

        public DocumentacionService(
            IDocumentacionRepository repository,
            IEstudianteRepository estudianteRepository,
            ISupabaseStorageService supabaseStorageService)
        {
            _repository = repository;
            _estudianteRepository = estudianteRepository;
            _supabaseStorageService = supabaseStorageService;
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
            var documentos =
                await _repository
                    .ObtenerDocumentosSolicitud(idSolicitud);

            foreach (var documento in documentos)
            {
                if (!string.IsNullOrWhiteSpace(
                    documento.RutaArchivo))
                {
                    documento.UrlArchivo =
                        await _supabaseStorageService
                            .ObtenerUrlPublica(
                                documento.RutaArchivo);
                }
            }

            return documentos;
        }

        public async Task<List<SolicitudDocumentacionDto>>
                ObtenerMisSolicitudes(int idUsuario)
        {
            var idEstudiante =
                await _estudianteRepository
                    .ObtenerIdEstudiantePorUsuario(idUsuario);

            if (idEstudiante == null)
                return new List<SolicitudDocumentacionDto>();

            return await _repository
                .ObtenerSolicitudesEstudiante(idEstudiante.Value);
        }
        public async Task<bool> SolicitudPerteneceAUsuario(
                int idSolicitud,
                int idUsuario)
        {
            return await _estudianteRepository
                .SolicitudPerteneceAEstudiante(
                    idSolicitud,
                    idUsuario);
        }
        public async Task<int> GuardarDocumentoSolicitud(
                int idSolicitud,
                int idDocumento,
                string rutaArchivo,
                string? observaciones)
        {
            return await _repository.GuardarDocumentoSolicitud(
                idSolicitud,
                idDocumento,
                rutaArchivo,
                observaciones);
        }
        public async Task<int> SubirDocumento(
                int idSolicitud,
                int idDocumento,
                Stream archivo,
                string nombreArchivo,
                string contentType,
                string? observaciones,
                int idUsuario)
        {
            var pertenece =
                await _estudianteRepository
                    .SolicitudPerteneceAEstudiante(
                        idSolicitud,
                        idUsuario);

            if (!pertenece)
            {
                throw new UnauthorizedAccessException(
                    "La solicitud no pertenece al estudiante autenticado.");
            }

            var carpeta =
                $"solicitudes/{idSolicitud}/{idDocumento}";

            var nombreUnico =
                $"{Guid.NewGuid()}_{nombreArchivo}";

            var rutaArchivo =
                await _supabaseStorageService.SubirArchivo(
                    archivo,
                    nombreUnico,
                    contentType,
                    carpeta);

            return await _repository.GuardarDocumentoSolicitud(
                idSolicitud,
                idDocumento,
                rutaArchivo,
                observaciones);
        }
    }
}