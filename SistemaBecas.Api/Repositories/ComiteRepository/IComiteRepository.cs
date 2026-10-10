using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.ComiteRepository
{
    public interface IComiteRepository
    {
        Task<List<ComiteDto>> ObtenerComitesAsync();

        Task<ComiteDto?> ObtenerComitePorIdAsync(int idComite);

        Task<int> CrearComiteAsync(ComiteDto comite);

        Task<bool> ActualizarComiteAsync(ComiteDto comite);

        Task<bool> AsignarEvaluadoresAsync(
            int idComite,
            List<int> idEvaluadores);

        Task<List<EvaluadorDto>> ObtenerEvaluadoresAsync();
    }
}