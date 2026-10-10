using SistemaBecas.Api.Repositories.ComiteRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.ComiteService
{
    public class ComiteService : IComiteService
    {
        private readonly IComiteRepository _comiteRepository;

        public ComiteService(IComiteRepository comiteRepository)
        {
            _comiteRepository = comiteRepository;
        }

        public async Task<List<ComiteDto>> ObtenerComitesAsync()
        {
            return await _comiteRepository.ObtenerComitesAsync();
        }

        public async Task<ComiteDto?> ObtenerComitePorIdAsync(int idComite)
        {
            return await _comiteRepository.ObtenerComitePorIdAsync(idComite);
        }

        public async Task<int> CrearComiteAsync(ComiteDto comite)
        {
            var idComite = await _comiteRepository.CrearComiteAsync(comite);

            await _comiteRepository.AsignarEvaluadoresAsync(
                idComite,
                comite.IdEvaluadores);

            return idComite;
        }

        public async Task<bool> ActualizarComiteAsync(ComiteDto comite)
        {
            var actualizado =
                await _comiteRepository.ActualizarComiteAsync(comite);

            if (!actualizado)
            {
                return false;
            }

            return await _comiteRepository.AsignarEvaluadoresAsync(
                comite.IdComite,
                comite.IdEvaluadores);
        }

        public async Task<bool> AsignarEvaluadoresAsync(
            int idComite,
            List<int> idEvaluadores)
        {
            return await _comiteRepository.AsignarEvaluadoresAsync(
                idComite,
                idEvaluadores);
        }

        public async Task<List<EvaluadorDto>> ObtenerEvaluadoresAsync()
        {
            return await _comiteRepository.ObtenerEvaluadoresAsync();
        }
    }
}