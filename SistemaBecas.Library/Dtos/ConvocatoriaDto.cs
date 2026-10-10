namespace SistemaBecas.Library.Dtos;

public class ConvocatoriaDto
{
    public int IdConvocatoria { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Cupos { get; set; }
    public string Estado { get; set; } = "";
}

public class ConvocatoriaGuardarDto
{
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Cupos { get; set; }
    public string Estado { get; set; } = "";
}
