using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento> AddEventos(Evento model);
        Task<Evento> UpdateEvento(int id, Evento model);
        Task<bool> DeleteEvento(int id);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestranes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestranes = false);
        Task<Evento> GetEventosByIdAsync(int id, bool includePalestranes = false);
    }
}
