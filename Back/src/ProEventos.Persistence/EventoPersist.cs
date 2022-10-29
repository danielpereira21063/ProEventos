using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;
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

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestranes)
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
                .OrderBy(evento => evento.Id)
                .Where(evento =>
                    evento.UserId == userId
                    && (evento.Tema.ToLower().Contains(pageParams.Term.ToLower()) || evento.Local.ToLower().Contains(pageParams.Term.ToLower())));

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
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
