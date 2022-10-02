using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestranes = false);
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestranes = false);
        Task<Evento> GetEventosByIdAsync(int userId, int id, bool includePalestranes = false);
    }
}
