namespace SistemaBecas.Api.Repositories.RecuperacionPassword
{
    public interface IRecuperacionPasswordRepository
    {
        Task<int?> ObtenerIdUsuarioPorCorreo(string correo);

        Task<bool> GuardarCodigo(
            int idUsuario,
            string codigo);

        Task<bool> VerificarCodigo(
            string correo,
            string codigo);

        Task<bool> CambiarPassword(
            string correo,
            string codigo,
            string passwordHash);
    }
}
