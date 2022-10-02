using ProEventos.Application.Dtos;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int id);
        Task<EventoDto[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestranes = false);
        Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestranes = false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int id, bool includePalestranes = false);
    }
}
