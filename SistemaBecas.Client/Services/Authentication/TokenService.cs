namespace SistemaBecas.Client.Services.Authentication
{
    public class TokenService
    {
        private string? _token;

        public string? ObtenerToken()
        {
            Console.WriteLine(
                $"TOKEN SERVICE - OBTENER: {!string.IsNullOrWhiteSpace(_token)}");

            return _token;
        }

        public void GuardarToken(string token)
        {
            _token = token;

            Console.WriteLine(
                $"TOKEN SERVICE - GUARDAR: {!string.IsNullOrWhiteSpace(_token)}");
        }

        public void EliminarToken()
        {
            _token = null;

            Console.WriteLine("TOKEN SERVICE - ELIMINADO");
        }
    }
}