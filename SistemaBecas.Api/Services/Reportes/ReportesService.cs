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

        public async Task<List<SolicitudesPorConvocatoriaDto>> ObtenerSolicitudesPorConvocatoria()
        {
            return await _reportesRepository.ObtenerSolicitudesPorConvocatoria();
        }

        public async Task<List<EstadoSolicitudesDto>> ObtenerEstadoSolicitudes()
        {
            return await _reportesRepository
                .ObtenerEstadoSolicitudes();
        }
        public async Task<List<BecasOtorgadasDto>>
            ObtenerBecasOtorgadas()
        {
            return await _reportesRepository
                .ObtenerBecasOtorgadas();
        }
    }
}
