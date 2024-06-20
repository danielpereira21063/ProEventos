using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService RedeSocialService,
                                      IEventoService eventoService,
                                      IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
            _redeSocialService = RedeSocialService;
            _eventoService = eventoService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {

            if (!(await AutorEvento(eventoId)))
                return NoContent();

            var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return NoContent();

            var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);

        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {

            if (!(await AutorEvento(eventoId)))
                return NoContent();

            var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);

        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {

            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return NoContent();

            var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
            if (redeSocial == null) return NoContent();

            return Ok(redeSocial);

        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {

            if (!await AutorEvento(eventoId))
                return NoContent();

            var RedeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (RedeSocial == null) return NoContent();

            return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId)
                   ? Ok(new { message = "Rede Social Deletada" })
                   : throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Evento.");

        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
            if (palestrante == null) return NoContent();

            var RedeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
            if (RedeSocial == null) return NoContent();

            return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId)
                   ? Ok(new { message = "Rede Social Deletada" })
                   : throw new Exception("Ocorreu um problem não específico ao tentar deletar Rede Social por Palestrante.");

        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null) return false;

            return true;
        }
    }
}
