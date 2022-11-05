using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int id);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestranes = false);
        Task<EventoDto> GetEventosByIdAsync(int userId, int id, bool includePalestranes = false);
    }
}
