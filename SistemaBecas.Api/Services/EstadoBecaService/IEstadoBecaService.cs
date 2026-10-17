using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.EstadoBecaService
{
    public interface IEstadoBecaService
    {
        Task<EstadoBecaDto?> ObtenerEstadoBecaAsync(int idSolicitud);
    }
}