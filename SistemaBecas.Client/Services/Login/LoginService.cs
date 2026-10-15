using System.Net.Http.Json;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginRespuestaDto?> LoginAsync(
            LoginPeticiónDto peticion)
        {
            var httpClient = _httpClientFactory.CreateClient("Api");

            var response = await httpClient.PostAsJsonAsync(
                "api/Login/login",
                peticion);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<LoginRespuestaDto>();
        }
    }
}