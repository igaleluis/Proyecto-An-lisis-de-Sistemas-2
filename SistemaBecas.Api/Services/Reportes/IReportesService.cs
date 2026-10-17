using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Services.Reportes
{
    public interface IReportesService
    {
        Task<List<SolicitudesPorConvocatoriaDto>>
            ObtenerSolicitudesPorConvocatoria(
                DateTime? fechaInicio,
                DateTime? fechaFin);

        Task<List<EstadoSolicitudesDto>>
            ObtenerEstadoSolicitudes(
                DateTime? fechaInicio,
                DateTime? fechaFin);

        Task<List<BecasOtorgadasDto>>
            ObtenerBecasOtorgadas(
                DateTime? fechaInicio,
                DateTime? fechaFin);

        Task<List<ConvocatoriasReporteDto>>
            ObtenerConvocatorias(
                DateTime? fechaInicio,
                DateTime? fechaFin);

        Task<EstadisticasReportesDto> ObtenerEstadisticas(
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}
