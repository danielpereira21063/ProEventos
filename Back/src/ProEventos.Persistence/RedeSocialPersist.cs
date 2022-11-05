using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    public class RedeSocialPersist : GeralPersist, IRedeSocialPersist
    {
        private readonly ProEventosContext _context;

        public RedeSocialPersist(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
                .AsNoTracking();

            query = query.Where(x => x.EventoId == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
                .AsNoTracking();

            return await query.Where(x => x.EventoId == palestranteId).ToArrayAsync();
        }

        public async Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
                .AsNoTracking();

            return await _context.RedesSociais.FirstOrDefaultAsync(x => x.Id == id && x.EventoId == eventoId);
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = _context.RedesSociais
            .AsNoTracking();

            return await _context.RedesSociais.FirstOrDefaultAsync(x => x.Id == id && x.PalestranteId == palestranteId);
        }
    }
}
