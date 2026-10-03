using Microsoft.EntityFrameworkCore;
using SistemaBecas.Api.Data;
using SistemaBecas.Api.Repositories.DocumentacionRepository;
using SistemaBecas.Library.Dtos;

namespace SistemaBecas.Api.Repositories.Documentacion
{
    public class DocumentacionRepository : IDocumentacionRepository
    {
        private readonly BecasDbContext _context;

        public DocumentacionRepository(BecasDbContext context)
        {
            _context = context;
        }

        public async Task<List<SolicitudDocumentacionDto>>
            ObtenerSolicitudesEstudiante(int idEstudiante)
        {
            return await _context.Solicituds
                .Where(s => s.Idestudiante == idEstudiante)
                .OrderByDescending(s => s.Fechasolicitud)
                .Select(s => new SolicitudDocumentacionDto
                {
                    IdSolicitud = s.Idsolicitud,
                    IdConvocatoria = s.Idconvocatoria,
                    FechaSolicitud = s.Fechasolicitud,
                    Estado = s.Estado
                })
                .ToListAsync();
        }

        public async Task<List<DocumentoSolicitudDto>>
            ObtenerDocumentosSolicitud(int idSolicitud)
        {
            return await _context.Solicituddocumentos
                .Where(sd => sd.Idsolicitud == idSolicitud)
                .Include(sd => sd.IddocumentoNavigation)
                .OrderBy(sd => sd.Iddocumento)
                .Select(sd => new DocumentoSolicitudDto
                {
                    IdSolicitudDocumento = sd.Idsolicituddocumento,
                    IdSolicitud = sd.Idsolicitud,
                    IdDocumento = sd.Iddocumento,

                    Nombre = sd.IddocumentoNavigation.Nombre,

                    Descripcion =
                        sd.IddocumentoNavigation.Descripcion,

                    Obligatorio =
                        sd.IddocumentoNavigation.Obligatorio,

                    RutaArchivo = sd.Rutaarchivo,

                    FechaCarga = sd.Fechacarga,

                    Estado = sd.Estado,

                    Observaciones = sd.Observaciones
                })
                .ToListAsync();
        }
    }
}