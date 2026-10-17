using SistemaBecas.Api.Repositories.EvaluacionRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.EvaluacionService
{
    public class EvaluacionService : IEvaluacionService
    {
        private readonly IEvaluacionRepository _evaluacionRepository;

        public EvaluacionService(IEvaluacionRepository evaluacionRepository)
        {
            _evaluacionRepository = evaluacionRepository;
        }

        public async Task<List<EvaluacionDto>> ObtenerEvaluacionesAsync()
        {
            return await _evaluacionRepository.ObtenerEvaluacionesAsync();
        }

        public async Task<EvaluacionDto?> ObtenerEvaluacionPorIdAsync(int idEvaluacion)
        {
            return await _evaluacionRepository.ObtenerEvaluacionPorIdAsync(idEvaluacion);
        }

        public async Task<int> CrearEvaluacionAsync(EvaluacionDto evaluacion)
        {
            return await _evaluacionRepository.CrearEvaluacionAsync(evaluacion);
        }

        public async Task<bool> ActualizarEvaluacionAsync(EvaluacionDto evaluacion)
        {
            return await _evaluacionRepository.ActualizarEvaluacionAsync(evaluacion);
        }
    }
}