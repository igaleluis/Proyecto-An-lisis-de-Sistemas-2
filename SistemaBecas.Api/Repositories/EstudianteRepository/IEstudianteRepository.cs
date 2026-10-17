namespace SistemaBecas.Api.Repositories.EstudianteRepository
{
    public interface IEstudianteRepository
    {
        Task<int?> ObtenerIdEstudiantePorUsuario(int idUsuario);

        Task<bool> SolicitudPerteneceAEstudiante(
           int idSolicitud,
           int idUsuario);
    }
}
