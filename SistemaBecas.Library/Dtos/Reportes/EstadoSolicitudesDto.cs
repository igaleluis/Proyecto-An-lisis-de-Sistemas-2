namespace SistemaBecas.Library.Dtos.Reportes
{
    public class EstadoSolicitudesDto
    {
        public int IdSolicitud { get; set; }

        public string Estudiante { get; set; } = string.Empty;

        public string Convocatoria { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string Observaciones { get; set; } = string.Empty;
    }
}