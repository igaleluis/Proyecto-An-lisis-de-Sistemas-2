namespace SistemaBecas.Client.Components.Pages
{
    public class SolicitudModelo
    {
        public int IdSolicitud { get; set; }

        public string Estudiante { get; set; } = string.Empty;

        public string DPI { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        public int IdConvocatoria { get; set; }

        public string Convocatoria { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }

        public string Estado { get; set; } = string.Empty;

        public decimal Punteo { get; set; }

        public string Observaciones { get; set; } = string.Empty;

        public string ObservacionesEvaluacion { get; set; } = string.Empty;
    }
}
