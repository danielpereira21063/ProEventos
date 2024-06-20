using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;

        public PalestrantesController(IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
        {

            var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
            if (palestrantes == null) return NoContent();

            Response.AddPagination(palestrantes.CurrentPage,
                                   palestrantes.PageSize,
                                   palestrantes.TotalCount,
                                   palestrantes.TotalPages);

            return Ok(palestrantes);

        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes()
        {

            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
            if (palestrante == null) return NoContent();

            return Ok(palestrante);

        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {

            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
            if (palestrante == null)
                palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model);

            return Ok(palestrante);

        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model)
        {
            var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);
            if (palestrante == null) return NoContent();

            return Ok(palestrante);

        }
    }
}
