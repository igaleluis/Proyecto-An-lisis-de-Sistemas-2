namespace SistemaBecas.Library.Dtos
{
    public class EvaluadorDto
    {
        public int IdEvaluador { get; set; }

        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Cargo { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
}