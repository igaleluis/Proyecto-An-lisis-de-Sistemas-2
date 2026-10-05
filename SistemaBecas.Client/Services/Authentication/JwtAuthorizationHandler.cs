using System.Net.Http.Headers;

namespace SistemaBecas.Client.Services.Authentication
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public JwtAuthorizationHandler(TokenService tokenService)
        {
            _tokenService = tokenService;

            Console.WriteLine("HANDLER - TOKEN SERVICE INYECTADO");
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = _tokenService.ObtenerToken();

            Console.WriteLine(
                $"TOKEN EN HANDLER: " +
                $"{(!string.IsNullOrWhiteSpace(token) ? "SI" : "NO")}");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);

                Console.WriteLine("BEARER AGREGADO");
            }

            return base.SendAsync(
                request,
                cancellationToken);
        }
    }
}