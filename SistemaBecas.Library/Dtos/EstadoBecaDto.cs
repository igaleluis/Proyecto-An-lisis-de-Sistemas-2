namespace SistemaBecas.Library.Dtos
{
    public class EstadoBecaDto
    {
        public int IdSolicitud { get; set; }

        public string Estudiante { get; set; } = "";

        public string Convocatoria { get; set; } = "";

        public DateTime FechaSolicitud { get; set; }

        public string EstadoSolicitud { get; set; } = "";

        public List<HistorialEstadoDto> Historial { get; set; } = new();

        public BecaEstadoDto? Beca { get; set; }
    }

    public class HistorialEstadoDto
    {
        public string EstadoAnterior { get; set; } = "";

        public string EstadoNuevo { get; set; } = "";

        public DateTime FechaCambio { get; set; }
    }

    public class BecaEstadoDto
    {
        public string Estado { get; set; } = "";

        public decimal? Monto { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string Observaciones { get; set; } = "";
    }
}