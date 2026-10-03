using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Client.Services.RegistroService
{
    public class RegistroService : IRegistroService
    {
        private readonly IHttpClientFactory _httpClient;
        public RegistroService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UsuarioDto?> RegistrarEstudiante(RegistroEstudianteDto registro)
        {
            var httpClient = _httpClient.CreateClient("Api");

            var response = await httpClient.PostAsJsonAsync(
                "api/Registro/RegistroEstudiante",
                registro);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<UsuarioDto>();
        }
    }
}
