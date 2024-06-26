﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _loteService;

        public LotesController(ILoteService LoteService)
        {
            _loteService = LoteService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {

            var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
            if (lotes == null) return NoContent();

            return Ok(lotes);

        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {

            var lotes = await _loteService.SaveLotes(eventoId, models);
            if (lotes == null) return NoContent();

            return Ok(lotes);

        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {

            var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
            if (lote == null) return NoContent();

            return await _loteService.DeleteLote(lote.EventoId, lote.Id)
                   ? Ok(new { message = "Lote Deletado" })
                   : throw new Exception("Ocorreu um problem não específico ao tentar deletar Lote.");

        }
    }
}
