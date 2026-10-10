using SistemaBecas.Library.Dtos.Reportes;

namespace SistemaBecas.Api.Services.Reportes
{
    public interface IReportesService
    {
        Task<List<SolicitudesPorConvocatoriaDto>> ObtenerSolicitudesPorConvocatoria();

        Task<List<EstadoSolicitudesDto>> ObtenerEstadoSolicitudes();
        Task<List<BecasOtorgadasDto>> ObtenerBecasOtorgadas();
    }
}
