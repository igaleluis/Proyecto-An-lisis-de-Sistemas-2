using Microsoft.Extensions.Options;
using SistemaBecas.Api.Configuration;
using System.Net;
using System.Net.Mail;

namespace SistemaBecas.Api.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task EnviarCorreoAsync(
            string destinatario,
            string asunto,
            string cuerpo)
        {
            using var smtpClient = new SmtpClient(
                _emailSettings.Host,
                _emailSettings.Port);

            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential(
                _emailSettings.Username,
                _emailSettings.Password);

            using var mensaje = new MailMessage();

            mensaje.From = new MailAddress(
                _emailSettings.From);

            mensaje.To.Add(destinatario);

            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mensaje);
        }
    }
}
