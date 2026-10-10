using SistemaBecas.Api.Configuration;

namespace SistemaBecas.Api.Services.SupabaseStorage
{
    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;

        public SupabaseStorageService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _settings = configuration
                .GetSection("Supabase")
                .Get<SupabaseSettings>()
                ?? throw new InvalidOperationException(
                    "No se encontró la configuración de Supabase.");
        }

        public async Task<string> SubirArchivo(
            Stream archivo,
            string nombreArchivo,
            string contentType,
            string carpeta)
        {
            var rutaArchivo =
                $"{carpeta}/{nombreArchivo}";

            var url =
                $"{_settings.Url.TrimEnd('/')}/storage/v1/object/" +
                $"{_settings.BucketName}/{rutaArchivo}";

            using var contenido =
                new StreamContent(archivo);

            contenido.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    contentType);

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    url);

            request.Headers.Add(
                "Authorization",
                $"Bearer {_settings.ServiceRoleKey}");

            request.Headers.Add(
                "apikey",
                _settings.ServiceRoleKey);

            request.Content = contenido;

            var response =
                await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content.ReadAsStringAsync();
                Console.WriteLine("==============================");
                Console.WriteLine("ERROR SUPABASE STORAGE");
                Console.WriteLine($"STATUS: {response.StatusCode}");
                Console.WriteLine($"DETALLE: {error}");
                Console.WriteLine("==============================");

                throw new InvalidOperationException(
                    $"No fue posible subir el archivo a Supabase Storage. " +
                    $"Código: {response.StatusCode}. " +
                    $"Detalle: {error}");
            }

            return rutaArchivo;
        }

        public Task<string> ObtenerUrlPublica(
             string rutaArchivo)
        {
            var url =
                $"{_settings.Url.TrimEnd('/')}" +
                $"/storage/v1/object/public/" +
                $"{_settings.BucketName}/" +
                $"{rutaArchivo}";

            return Task.FromResult(url);
        }
    }
}
