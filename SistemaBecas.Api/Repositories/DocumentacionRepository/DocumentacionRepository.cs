using Npgsql;
using SistemaBecas.Api.Repositories.DocumentacionRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.Documentacion
{
    public class DocumentacionRepository : IDocumentacionRepository
    {
        private readonly DbConnectionBecas _db;

        public DocumentacionRepository(DbConnectionBecas db)
        {
            _db = db;
        }

        public async Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante)
        {
            var solicitudes = new List<SolicitudDocumentacionDto>();

            using var connection = _db.CreateConnection();

            await connection.OpenAsync();

            const string sql = """
                SELECT
                    s.idsolicitud,
                    s.idconvocatoria,
                    s.fechasolicitud,
                    s.estado
                FROM public.solicitud s
                WHERE s.idestudiante = @IdEstudiante
                ORDER BY s.fechasolicitud DESC;
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@IdEstudiante",
                idEstudiante);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                solicitudes.Add(new SolicitudDocumentacionDto
                {
                    IdSolicitud = reader.GetInt32(0),
                    IdConvocatoria = reader.GetInt32(1),
                    FechaSolicitud = reader.GetDateTime(2),
                    Estado = reader.GetString(3)
                });
            }

            return solicitudes;
        }


        public async Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud)
        {
            var documentos = new List<DocumentoSolicitudDto>();

            using var connection = _db.CreateConnection();

            await connection.OpenAsync();

            const string sql = """
                SELECT
                    sd.idsolicituddocumento,
                    sd.idsolicitud,
                    sd.iddocumento,
                    d.nombre,
                    d.descripcion,
                    d.obligatorio,
                    sd.rutaarchivo,
                    sd.fechacarga,
                    sd.estado,
                    sd.observaciones
                FROM public.solicituddocumento sd
                INNER JOIN public.documento d
                    ON d.iddocumento = sd.iddocumento
                WHERE sd.idsolicitud = @IdSolicitud
                ORDER BY d.iddocumento;
                """;

            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "@IdSolicitud",
                idSolicitud);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                documentos.Add(new DocumentoSolicitudDto
                {
                    IdSolicitudDocumento = reader.GetInt32(0),
                    IdSolicitud = reader.GetInt32(1),
                    IdDocumento = reader.GetInt32(2),
                    Nombre = reader.GetString(3),
                    Descripcion = reader.IsDBNull(4)
                        ? null
                        : reader.GetString(4),
                    Obligatorio = reader.GetBoolean(5),
                    RutaArchivo = reader.IsDBNull(6)
                        ? null
                        : reader.GetString(6),
                    FechaCarga = reader.IsDBNull(7)
                        ? null
                        : reader.GetDateTime(7),
                    Estado = reader.GetString(8),
                    Observaciones = reader.IsDBNull(9)
                        ? null
                        : reader.GetString(9)
                });
            }

            return documentos;
        }
    }
}