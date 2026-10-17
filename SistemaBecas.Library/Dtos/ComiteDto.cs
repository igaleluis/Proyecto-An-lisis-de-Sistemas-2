namespace SistemaBecas.Library.Dtos
{
    public class ComiteDto
    {
        public int IdComite { get; set; }

        public string Nombre { get; set; } = "";

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; } = "";

        public string Estado { get; set; } = "";

        public List<int> IdEvaluadores { get; set; } = new();
    }
}