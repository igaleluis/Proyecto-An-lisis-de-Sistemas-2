namespace SistemaBecas.Api.Services.SupabaseStorage
{
    public interface ISupabaseStorageService
    {
        Task<string> SubirArchivo(
            Stream archivo,
            string nombreArchivo,
            string contentType,
            string carpeta);

        Task<string> ObtenerUrlPublica(string rutaArchivo);
    }
}
