using SistemaBecas.Api.Repositories.RecuperacionPassword;
using SistemaBecas.Api.Services.Email;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.RecuperacionPassword
{
    public class RecuperacionPasswordService
         : IRecuperacionPasswordService
    {
        private readonly IRecuperacionPasswordRepository
            _repository;

        private readonly IEmailService
            _emailService;

        public RecuperacionPasswordService(
            IRecuperacionPasswordRepository repository,
            IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<bool> SolicitarRecuperacion(
            RecuperarContraseñaDto dto)
        {
            var idUsuario =
                await _repository.ObtenerIdUsuarioPorCorreo(
                    dto.Correo);

            if (idUsuario == null)
            {
                return false;
            }

            var codigo = Random.Shared
                .Next(100000, 1000000)
                .ToString();

            var guardado =
                await _repository.GuardarCodigo(
                    idUsuario.Value,
                    codigo);

            if (!guardado)
            {
                return false;
            }

            var cuerpo = $"""
                <h2>Recuperación de contraseña</h2>

                <p>Has solicitado recuperar la contraseña
                de tu cuenta del Sistema de Becas.</p>

                <p>Tu código de recuperación es:</p>

                <h1>{codigo}</h1>

                <p>Este código tiene una validez de
                <strong>15 minutos</strong>.</p>

                <p>Si no solicitaste este cambio,
                puedes ignorar este correo.</p>
                """;

            await _emailService.EnviarCorreoAsync(
                dto.Correo,
                "Recuperación de contraseña - Sistema de Becas",
                cuerpo);

            return true;
        }

        public async Task<bool> VerificarCodigo(
            VerificarCodigoDto dto)
        {
            return await _repository.VerificarCodigo(
                dto.Correo,
                dto.Codigo);
        }

        public async Task<bool> CambiarPassword(
            CambiarPasswordDto dto)
        {
            var codigoValido =
                await _repository.VerificarCodigo(
                    dto.Correo,
                    dto.Codigo);

            if (!codigoValido)
            {
                return false;
            }

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.NuevaPassword);

            return await _repository.CambiarPassword(
                dto.Correo,
                dto.Codigo,
                passwordHash);
        }
    }
}
