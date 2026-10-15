namespace SistemaBecas.Api.Services.Email
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string cuerpo);
    }
}
