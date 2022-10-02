using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventoPersist(ProEventosContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestranes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .AsNoTracking()
                .Where(u => u.UserId == userId); ;

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante)
                    .Include(x => x.Lotes);
            }

            query = query.OrderBy(x => x.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestranes)
        {
            IQueryable<Evento> query = _context.Eventos
                .AsNoTracking();

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante);
            }

            query = query
                .OrderBy(x => x.Id)
                .Where(x => x.Tema.ToLower().Contains(tema.ToLower()) && x.UserId == userId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosByIdAsync(int userId, int id, bool includePalestranes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Where(u => u.UserId == userId)
                .AsNoTracking();

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante);
            }

            query = query
                .Include(x => x.Lotes)
                .Where(x => x.Id == id && x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
