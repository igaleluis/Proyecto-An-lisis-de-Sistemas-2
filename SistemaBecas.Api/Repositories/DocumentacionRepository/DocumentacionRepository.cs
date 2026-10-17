using Microsoft.EntityFrameworkCore;
using SistemaBecas.Api.Data;
using SistemaBecas.Api.Entities;
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
            return await _context.Documentos
                .Where(d => d.Estado == "Activo")
                .Select(d => new DocumentoSolicitudDto
                {
                    IdSolicitudDocumento = d.Solicituddocumentos
                        .Where(sd => sd.Idsolicitud == idSolicitud)
                        .Select(sd => sd.Idsolicituddocumento)
                        .FirstOrDefault(),

                    IdSolicitud = idSolicitud,

                    IdDocumento = d.Iddocumento,

                    Nombre = d.Nombre,

                    Descripcion = d.Descripcion,

                    Obligatorio = d.Obligatorio,

                    RutaArchivo = d.Solicituddocumentos
                        .Where(sd => sd.Idsolicitud == idSolicitud)
                        .Select(sd => sd.Rutaarchivo)
                        .FirstOrDefault(),

                    FechaCarga = d.Solicituddocumentos
                        .Where(sd => sd.Idsolicitud == idSolicitud)
                        .Select(sd => sd.Fechacarga)
                        .FirstOrDefault(),

                    Estado = d.Solicituddocumentos
                        .Where(sd => sd.Idsolicitud == idSolicitud)
                        .Select(sd => sd.Estado)
                        .FirstOrDefault() ?? "Pendiente",

                    Observaciones = d.Solicituddocumentos
                        .Where(sd => sd.Idsolicitud == idSolicitud)
                        .Select(sd => sd.Observaciones)
                        .FirstOrDefault()
                })
                .OrderBy(d => d.IdDocumento)
                .ToListAsync();
        }
        public async Task<int> GuardarDocumentoSolicitud(
                int idSolicitud,
                int idDocumento,
                string rutaArchivo,
                string? observaciones)
        {
            var documento = new Solicituddocumento
            {
                Idsolicitud = idSolicitud,
                Iddocumento = idDocumento,
                Rutaarchivo = rutaArchivo,
                Fechacarga = DateTime.Now,
                Estado = "Cargado",
                Observaciones = observaciones
            };

            _context.Solicituddocumentos.Add(documento);

            await _context.SaveChangesAsync();

            return documento.Idsolicituddocumento;
        }
    }

}