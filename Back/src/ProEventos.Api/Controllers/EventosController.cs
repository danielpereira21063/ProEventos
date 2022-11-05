using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IUtil _util;
        private readonly string _destino = "Images";

        public EventosController(IEventoService eventoService, IUtil util)
        {
            _eventoService = eventoService;
            _util = util;
        }


        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.AddEventos(User.GetUserId(), model);

                if (evento == null)
                {
                    return NoContent();
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"Erro ao adicionar evento. \n{ex.Message}");
            }
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), eventoId);

                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemUrl, _destino);
                    evento.ImagemUrl = await _util.SaveImage(file, _destino);
                }

                var eventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"Erro ao adicionar evento. \n{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);

                if (evento == null)
                {
                    return NoContent();
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"Erro ao atualizar evento. \n{ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id);

                if (evento == null) return NoContent();

                if (!await _eventoService.DeleteEvento(User.GetUserId(), id))
                {
                    throw new Exception("Ocorreu um erro desconhecido ao deletar o evento.");
                }

                _util.DeleteImage(evento.ImagemUrl, _destino);
                return Ok(new
                {
                    message = "deletado",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar evento. \n{ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, includePalestranes: true);

                if (eventos == null)
                {
                    return NoContent();
                }

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. \n{ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventosByIdAsync(User.GetUserId(), id, true);
                if (evento == null)
                {
                    return NoContent();
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar evento. \n{ex.Message}");
            }
        }
    }
}
