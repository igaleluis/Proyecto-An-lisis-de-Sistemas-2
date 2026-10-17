namespace SistemaBecas.Library.Dtos;

public class SolicitudGestionDto
{
    public int IdSolicitud { get; set; }
    public int IdConvocatoria { get; set; }
    public int IdEstudiante { get; set; }
    public string Estudiante { get; set; } = "";
    public string Dpi { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Convocatoria { get; set; } = "";
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = "";
    public string Observaciones { get; set; } = "";
    public decimal? Punteo { get; set; }
}

public class SolicitudEstadoDto
{
    public string Estado { get; set; } = "";
    public string Observaciones { get; set; } = "";
}
