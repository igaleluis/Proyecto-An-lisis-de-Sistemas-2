namespace SistemaBecas.Library.Dtos
{
    public class DocumentoSolicitudDto
    {
        public int IdSolicitudDocumento { get; set; }

        public int IdSolicitud { get; set; }

        public int IdDocumento { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Obligatorio { get; set; }

        public string? RutaArchivo { get; set; }

        public DateTime? FechaCarga { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }
}