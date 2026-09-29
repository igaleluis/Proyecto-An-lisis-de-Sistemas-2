namespace SistemaBecas.Client.Models
{
    public class SolicitudDto
    {
        public int IdSolicitud { get; set; }

        public string Estudiante { get; set; } = "";

        public string DPI { get; set; } = "";

        public string Correo { get; set; } = "";

        public string Telefono { get; set; } = "";

        public string Direccion { get; set; } = "";

        public string Convocatoria { get; set; } = "";

        public DateTime FechaSolicitud { get; set; }

        public string Estado { get; set; } = "";

        public List<SolicitudDocumentoDto> Documentos { get; set; } = new();

        public List<EvaluacionDto> Evaluaciones { get; set; } = new();
    }

    public class SolicitudDocumentoDto
    {
        public string Nombre { get; set; } = "";

        public bool Obligatorio { get; set; }

        public string RutaArchivo { get; set; } = "";

        public string Estado { get; set; } = "";
    }

    public class EvaluacionDto
    {
        public string Evaluador { get; set; } = "";

        public decimal Punteo { get; set; }

        public DateTime FechaEvaluacion { get; set; }

        public string Observaciones { get; set; } = "";
    }
}
