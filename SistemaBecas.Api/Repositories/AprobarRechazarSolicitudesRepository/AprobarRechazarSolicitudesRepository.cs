namespace SistemaBecas.Api.Repositories.AprobarRechazarSolicitudesRepository
{
    public class AprobarRechazarSolicitudesRepository:IAprobarRechazarSolicitudesRepository
    {
        DbConnectionBecas _connection;
        public AprobarRechazarSolicitudesRepository(DbConnectionBecas connection)
        {
            _connection = connection;
        }
    }
}
