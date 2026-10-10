using SistemaBecas.Client.Services.Authentication;
using SistemaBecas.Library.Dtos.Reportes;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SistemaBecas.Client.Services.Reportes
{
    public class ReportesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenService _tokenService;

        public ReportesService(
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
                var client =
                    _httpClientFactory.CreateClient("Api");

                var token =
                    _tokenService.ObtenerToken();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);
                }

                return client;
            }
        }

        public async Task<List<SolicitudesPorConvocatoriaDto>>
            ObtenerSolicitudesPorConvocatoria()
        {
            var resultado =
                await Api.GetFromJsonAsync<
                    List<SolicitudesPorConvocatoriaDto>>(
                        "api/Reportes/solicitudes-por-convocatoria");

            return resultado ??
                new List<SolicitudesPorConvocatoriaDto>();
        }

        // REPORTE 1: GENERAR PDF DE SOLICITUDES POR CONVOCATORIA
        public async Task<byte[]> GenerarSolicitudesPorConvocatoriaPdf()
        {
            var response =
                await Api.GetAsync(
                    "api/Reportes/solicitudes-por-convocatoria/pdf");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // REPORTE 2: GENERAR PDF DE ESTADO DE SOLICITUDES
        public async Task<byte[]> GenerarEstadoSolicitudesPdf()
        {
            var response =
                await Api.GetAsync(
                    "api/Reportes/estado-solicitudes/pdf");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}