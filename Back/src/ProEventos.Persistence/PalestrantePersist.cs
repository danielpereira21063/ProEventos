using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : GeralPersist, IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }


        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query
                .Where(p => p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) ||
                    p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                    p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower()) &&
                    p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                .OrderBy(x => x.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }


        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(x => x.PalestranteEventos)
                    .ThenInclude(x => x.Evento);
            }

            query = query
                .OrderBy(x => x.Id)
                .Where(x => x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
