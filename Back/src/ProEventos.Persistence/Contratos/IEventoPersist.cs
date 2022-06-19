using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestranes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestranes = false);
        Task<Evento> GetEventosByIdAsync(int id, bool includePalestranes = false);
    }
}
