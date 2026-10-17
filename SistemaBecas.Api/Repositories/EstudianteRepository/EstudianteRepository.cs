using Microsoft.EntityFrameworkCore;
using SistemaBecas.Api.Data;

namespace SistemaBecas.Api.Repositories.EstudianteRepository
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly BecasDbContext _context;

        public EstudianteRepository(BecasDbContext context)
        {
            _context = context;
        }

        public async Task<int?> ObtenerIdEstudiantePorUsuario(
            int idUsuario)
        {
            return await _context.Estudiantes
                .Where(e =>
                    e.Idusuario == idUsuario &&
                    e.Estado == "Activo")
                .Select(e => (int?)e.Idestudiante)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> SolicitudPerteneceAEstudiante(
    int idSolicitud,
    int idUsuario)
        {
            return await _context.Solicituds
                .AnyAsync(s =>
                    s.Idsolicitud == idSolicitud &&
                    s.IdestudianteNavigation.Idusuario == idUsuario &&
                    s.IdestudianteNavigation.Estado == "Activo");
        }
    }
}