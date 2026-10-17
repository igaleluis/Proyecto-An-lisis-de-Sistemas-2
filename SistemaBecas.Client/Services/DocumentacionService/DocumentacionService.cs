
using System.Net.Http.Json;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.DocumentacionService
{
    public class DocumentacionService : IDocumentacionService
    {
        private readonly IHttpClientFactory _httpClient;

        public DocumentacionService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante)
        {
            var client = _httpClient.CreateClient("Api");

            var response = await client.GetAsync(
                $"api/Documentacion/solicitudes/{idEstudiante}");

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<SolicitudDocumentacionDto>>()
                ?? new List<SolicitudDocumentacionDto>();
        }

        public async Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud)
        {
            var client = _httpClient.CreateClient("Api");

            var response = await client.GetAsync(
                $"api/Documentacion/solicitud/{idSolicitud}/documentos");

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<DocumentoSolicitudDto>>()
                ?? new List<DocumentoSolicitudDto>();
        }
    }
}
