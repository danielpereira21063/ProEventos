using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProEventos.API.Data;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly DataContext _context;

        public EventoController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Post(IEnumerable<Evento> eventos)
        {
            foreach(var evento in eventos)
            {
                _context.Eventos.Add(evento);
            }
            return Ok();
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            var eventos = _context.Eventos.ToList();
            return eventos;
        }
    }
}
