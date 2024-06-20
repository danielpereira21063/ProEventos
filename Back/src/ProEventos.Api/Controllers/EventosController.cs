using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.Dtos;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;
using ProEventos.Api.Helpers;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IHostingEnvironment _env;
        private readonly IUtil _util;

        private readonly string _destino = "Images";

        public EventosController(IEventoService eventoService,
                                 IUtil util,
                                 IAccountService accountService,
                                 IHostingEnvironment env)
        {
            _util = util;
            _env = env;
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {


                //CORRIGIR                //CORRIGIR                //CORRIGIR                //CORRIGIR
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var ms = new MemoryStream();                
                file.CopyTo(ms);
                var fileBytes = ms.ToArray(); ;
                string base4 = Convert.ToBase64String(fileBytes);
                evento.Imagem = Convert.FromBase64String(base4);

                //_util.DeleteImage(evento.Imagem, _destino);
                //evento.Imagem = await _util.SaveImage(file, _destino);
            }
            var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(EventoRetorno);

        }

        [HttpGet("image/get/{eventoId}")]
        public async Task<IActionResult> GetEventoImage(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
            if (evento == null) return NoContent();

            return File(evento.Imagem, "image/jpeg");
        }


        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            var evento = await _eventoService.AddEventos(User.GetUserId(), model);
            if (evento == null) return NoContent();

            return Ok(evento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
            if (evento == null) return NoContent();

            return Ok(evento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(User.GetUserId(), id))
            {
                //_util.DeleteImage(evento.Imagem, _destino);
                return Ok(new { message = "Deletado" });
            }
            else
            {
                throw new Exception("Ocorreu um problem não específico ao tentar deletar Evento.");
            }
        }
    }
}
