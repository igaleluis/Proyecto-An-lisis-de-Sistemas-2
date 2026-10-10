namespace SistemaBecas.Library.Dtos
{
    public class EvaluacionDto
    {
        public int IdEvaluacion { get; set; }

        public int IdSolicitud { get; set; }

        public int IdEvaluador { get; set; }

        public DateTime FechaEvaluacion { get; set; }

        public decimal? Punteo { get; set; }

        public string Observaciones { get; set; } = "";

        public string Estado { get; set; } = "";
    }
}