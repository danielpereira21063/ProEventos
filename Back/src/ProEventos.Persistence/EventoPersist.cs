using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
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

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestranes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking();

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante);
            }

            query = query.OrderBy(x => x.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestranes)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking();

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante);
            }

            query = query
                .OrderBy(x => x.Id)
                .Where(x => x.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventosByIdAsync(int id, bool includePalestranes)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking();

            if (includePalestranes)
            {
                query = query
                    .Include(x => x.PalestrantesEventos)
                    .ThenInclude(x => x.Palestrante);
            }

            query = query
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
