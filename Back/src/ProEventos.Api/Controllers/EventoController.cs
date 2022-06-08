using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Api.Models;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return new Evento[]
            {
                new Evento
                {
                    Tema = "Angular 11",
                    DataEvento = DateTime.Now.AddDays(7).ToString(),
                    EventoId = 1,
                    ImagemUrl = "",
                    Local = "Aqui",
                    Lote = "",
                    QtdPessoas = 250
                }
            };
        }
    }
}
