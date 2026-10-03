using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Services.RecuperacionPassword
{
    public interface IRecuperacionPasswordService
    {
        Task<bool> SolicitarRecuperacion(
            RecuperarContraseñaDto dto);

        Task<bool> VerificarCodigo(
            VerificarCodigoDto dto);

        Task<bool> CambiarPassword(
            CambiarPasswordDto dto);
    }
}