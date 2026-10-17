using SistemaBecas.Api.Repositories.EstadoBecaRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.EstadoBecaService
{
    public class EstadoBecaService : IEstadoBecaService
    {
        private readonly IEstadoBecaRepository _estadoBecaRepository;

        public EstadoBecaService(
            IEstadoBecaRepository estadoBecaRepository)
        {
            _estadoBecaRepository = estadoBecaRepository;
        }

        public async Task<EstadoBecaDto?> ObtenerEstadoBecaAsync(
            int idSolicitud)
        {
            return await _estadoBecaRepository
                .ObtenerEstadoBecaAsync(idSolicitud);
        }
    }
}