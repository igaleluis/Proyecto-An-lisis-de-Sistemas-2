
namespace SistemaBecas.Library.Dtos.Reportes
{
    public class ConvocatoriasReporteDto
    {
        public int IdConvocatoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int Cupos { get; set; }

        public string Estado { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }
    }
}
