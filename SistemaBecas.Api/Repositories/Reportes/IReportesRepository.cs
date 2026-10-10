using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Repositories.Reportes
{
    public interface IReportesRepository
    {
        Task<List<SolicitudesPorConvocatoriaDto>> ObtenerSolicitudesPorConvocatoria();
        Task<List<EstadoSolicitudesDto>> ObtenerEstadoSolicitudes();
        Task<List<BecasOtorgadasDto>> ObtenerBecasOtorgadas();
    }
}
