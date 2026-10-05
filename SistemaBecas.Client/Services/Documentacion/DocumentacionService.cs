using Microsoft.AspNetCore.Components.Forms;
using SistemaBecas.Client.Services.Authentication;
using SistemaBecas.Library.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SistemaBecas.Client.Services.Documentacion
{
    public class DocumentacionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenService _tokenService;

        public DocumentacionService(
            IHttpClientFactory httpClientFactory,
            TokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        private HttpClient Api
        {
            get
            {
                var client = _httpClientFactory.CreateClient("Api");

                var token = _tokenService.ObtenerToken();

                Console.WriteLine(
                    $"DOCUMENTACION SERVICE - TOKEN: " +
                    $"{(!string.IsNullOrWhiteSpace(token) ? "SI" : "NO")}");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);

                    Console.WriteLine(
                        "DOCUMENTACION SERVICE - BEARER AGREGADO");
                }

                return client;
            }
        }

        public async Task<List<SolicitudDocumentacionDto>>
            ObtenerMisSolicitudes()
        {
            var resultado =
                await Api.GetFromJsonAsync<
                    List<SolicitudDocumentacionDto>>(
                        "api/Documentacion/mis-solicitudes");

            return resultado ?? new List<SolicitudDocumentacionDto>();
        }

        public async Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud)
        {
            var resultado =
                await Api.GetFromJsonAsync<
                    List<DocumentoSolicitudDto>>(
                        $"api/Documentacion/solicitud/{idSolicitud}/documentos");

            return resultado ?? new List<DocumentoSolicitudDto>();
        }
        public async Task<bool> SubirDocumento(
            int idSolicitud,
            int idDocumento,
            IBrowserFile archivo,
            string? observaciones)
        {
            var token = _tokenService.ObtenerToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            using var contenido = new MultipartFormDataContent();

            contenido.Add(
                new StringContent(idSolicitud.ToString()),
                "idSolicitud");

            contenido.Add(
                new StringContent(idDocumento.ToString()),
                "idDocumento");

            contenido.Add(
                new StringContent(observaciones ?? string.Empty),
                "observaciones");

            var stream =
                archivo.OpenReadStream(
                    maxAllowedSize: 10 * 1024 * 1024);

            var contenidoArchivo =
                new StreamContent(stream);

            contenidoArchivo.Headers.ContentType =
                new MediaTypeHeaderValue(
                    archivo.ContentType);

            contenido.Add(
                contenidoArchivo,
                "archivo",
                archivo.Name);

            var client =
                _httpClientFactory.CreateClient("Api");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.PostAsync(
                    "api/Documentacion/subir-documento",
                    contenido);

            return response.IsSuccessStatusCode;
        }
    }
}