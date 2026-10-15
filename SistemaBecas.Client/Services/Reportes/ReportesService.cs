using SistemaBecas.Client.Services.Authentication;
using SistemaBecas.Library.Dtos.Reportes;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Globalization;

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

        // ============================================================
        // CLIENTE HTTP CON TOKEN JWT
        // ============================================================

        private HttpClient Api
        {
            get
            {
                var client = _httpClientFactory.CreateClient("Api");

                var token = _tokenService.ObtenerToken();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                return client;
            }
        }

        // ============================================================
        // CONSTRUIR PARÁMETROS DE FECHA
        // ============================================================

        private static string ConstruirParametrosFecha(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var parametros = new List<string>();

            if (fechaInicio.HasValue)
            {
                parametros.Add(
                    $"fechaInicio={Uri.EscapeDataString(
                        fechaInicio.Value.ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture))}");
            }

            if (fechaFin.HasValue)
            {
                parametros.Add(
                    $"fechaFin={Uri.EscapeDataString(
                        fechaFin.Value.ToString(
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture))}");
            }

            return parametros.Count > 0
                ? "?" + string.Join("&", parametros)
                : string.Empty;
        }

        // ============================================================
        // REPORTE 1: SOLICITUDES POR CONVOCATORIA
        // ============================================================

        public async Task<List<SolicitudesPorConvocatoriaDto>>
            ObtenerSolicitudesPorConvocatoria(
                DateTime? fechaInicio = null,
                DateTime? fechaFin = null)
        {
            var url = "api/Reportes/solicitudes-por-convocatoria"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var resultado =
                await Api.GetFromJsonAsync<
                    List<SolicitudesPorConvocatoriaDto>>(url);

            return resultado ??
                new List<SolicitudesPorConvocatoriaDto>();
        }

        public async Task<byte[]> GenerarSolicitudesPorConvocatoriaPdf(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var url = "api/Reportes/solicitudes-por-convocatoria/pdf"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var response = await Api.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // ============================================================
        // REPORTE 2: ESTADO DE SOLICITUDES
        // ============================================================

        public async Task<List<EstadoSolicitudesDto>>
            ObtenerEstadoSolicitudes(
                DateTime? fechaInicio = null,
                DateTime? fechaFin = null)
        {
            var url = "api/Reportes/estado-solicitudes"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var resultado =
                await Api.GetFromJsonAsync<
                    List<EstadoSolicitudesDto>>(url);

            return resultado ?? new List<EstadoSolicitudesDto>();
        }

        public async Task<byte[]> GenerarEstadoSolicitudesPdf(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var url = "api/Reportes/estado-solicitudes/pdf"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var response = await Api.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // ============================================================
        // REPORTE 3: BECAS OTORGADAS
        // ============================================================

        public async Task<byte[]> GenerarBecasOtorgadasPdf(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var url = "api/Reportes/becas-otorgadas/pdf"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var response = await Api.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // ============================================================
        // REPORTE 4: CONVOCATORIAS
        // ============================================================

        public async Task<List<ConvocatoriasReporteDto>>
            ObtenerConvocatorias(
                DateTime? fechaInicio = null,
                DateTime? fechaFin = null)
        {
            var url = "api/Reportes/convocatorias"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var resultado =
                await Api.GetFromJsonAsync<
                    List<ConvocatoriasReporteDto>>(url);

            return resultado ??
                new List<ConvocatoriasReporteDto>();
        }

        public async Task<byte[]> GenerarConvocatoriasPdf(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var url = "api/Reportes/convocatorias/pdf"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var response = await Api.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        // ============================================================
        // REPORTE 5: ESTADÍSTICAS
        // ============================================================

        public async Task<EstadisticasReportesDto> ObtenerEstadisticas(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null)
        {
            var url = "api/Reportes/estadisticas"
                + ConstruirParametrosFecha(fechaInicio, fechaFin);

            var resultado =
                await Api.GetFromJsonAsync<EstadisticasReportesDto>(url);

            return resultado ?? new EstadisticasReportesDto();
        }
    }
}
