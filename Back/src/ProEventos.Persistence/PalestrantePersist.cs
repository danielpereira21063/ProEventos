using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking();

            if (includeEventos)
            {
                query = query
                    .Include(x => x.PalestranteEventos)
                    .ThenInclude(x => x.Evento);
            }

            query = query
                .OrderBy(x => x.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking();

            if (includeEventos)
            {
                query = query
                    .Include(x => x.PalestranteEventos)
                    .ThenInclude(x => x.Evento);
            }

            query = query
                .OrderBy(x => x.Id)
                .Where(x => x.User.PrimeiroNome.ToLower().Contains(nome.ToLower()) || x.User.UltimoNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.AsNoTracking();

            if (includeEventos)
            {
                query = query
                    .Include(x => x.PalestranteEventos)
                    .ThenInclude(x => x.Evento);
            }

            query = query
                .OrderBy(x => x.Id)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
