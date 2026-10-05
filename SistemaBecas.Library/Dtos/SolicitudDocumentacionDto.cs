namespace SistemaBecas.Library.Dtos
{
    public class SolicitudDocumentacionDto
    {
        public int IdSolicitud { get; set; }

        public int IdConvocatoria { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? NombreConvocatoria { get; set; }
        public string TextoSolicitud =>
           $"Solicitud #{IdSolicitud:D4} - {FechaSolicitud:dd/MM/yyyy} - {Estado}";
    }
}
