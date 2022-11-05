using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IRedeSocialPersist : IGeralPersist
    {
        Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id);
        Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
    }
}
