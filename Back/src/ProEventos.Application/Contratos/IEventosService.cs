using ProEventos.Application.Dtos;
using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int id);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestranes = false);
        Task<EventoDto[]> GetAllEventosAsync(bool includePalestranes = false);
        Task<EventoDto> GetEventosByIdAsync(int id, bool includePalestranes = false);
    }
}
