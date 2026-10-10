using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.EvaluacionService
{
    public interface IEvaluacionService
    {
        Task<List<EvaluacionDto>> ObtenerEvaluacionesAsync();

        Task<EvaluacionDto?> ObtenerEvaluacionPorIdAsync(int idEvaluacion);

        Task<int> CrearEvaluacionAsync(EvaluacionDto evaluacion);

        Task<bool> ActualizarEvaluacionAsync(EvaluacionDto evaluacion);
    }
}