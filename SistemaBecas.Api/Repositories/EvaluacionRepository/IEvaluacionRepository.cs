using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.EvaluacionRepository
{
    public interface IEvaluacionRepository
    {
        Task<List<EvaluacionDto>> ObtenerEvaluacionesAsync();

        Task<EvaluacionDto?> ObtenerEvaluacionPorIdAsync(int idEvaluacion);

        Task<int> CrearEvaluacionAsync(EvaluacionDto evaluacion);

        Task<bool> ActualizarEvaluacionAsync(EvaluacionDto evaluacion);
    }
}