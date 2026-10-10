using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.EstadoBecaRepository
{
    public interface IEstadoBecaRepository
    {
        Task<EstadoBecaDto?> ObtenerEstadoBecaAsync(int idSolicitud);
    }
}