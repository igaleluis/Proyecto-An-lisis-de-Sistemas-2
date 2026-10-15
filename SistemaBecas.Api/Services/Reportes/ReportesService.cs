using SistemaBecas.Api.Repositories.Reportes;
using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Services.Reportes
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _reportesRepository;

        public ReportesService(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public Task<List<BecasOtorgadasDto>> ObtenerBecasOtorgadas(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return _reportesRepository.ObtenerBecasOtorgadas(
                fechaInicio,
                fechaFin);
        }

        public Task<List<ConvocatoriasReporteDto>> ObtenerConvocatorias(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return _reportesRepository.ObtenerConvocatorias(
                fechaInicio,
                fechaFin);
        }

        public Task<EstadisticasReportesDto> ObtenerEstadisticas(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return _reportesRepository.ObtenerEstadisticas(
                fechaInicio,
                fechaFin);
        }

        public Task<List<EstadoSolicitudesDto>> ObtenerEstadoSolicitudes(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            return _reportesRepository.ObtenerEstadoSolicitudes(
                fechaInicio,
                fechaFin);
        }

        public Task<List<SolicitudesPorConvocatoriaDto>>
            ObtenerSolicitudesPorConvocatoria(
                DateTime? fechaInicio,
                DateTime? fechaFin)
        {
            return _reportesRepository.ObtenerSolicitudesPorConvocatoria(
                fechaInicio,
                fechaFin);
        }
    }
}
