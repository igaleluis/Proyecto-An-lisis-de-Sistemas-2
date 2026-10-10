using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.ComiteService
{
    public interface IComiteService
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