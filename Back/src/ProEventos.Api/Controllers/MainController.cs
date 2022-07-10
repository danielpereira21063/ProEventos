using Microsoft.AspNetCore.Mvc;

namespace ProEventos.API.Controllers
{
    public abstract class MainController : ControllerBase
    {
        protected INotificador Notificador { get; }
        public MainController(INotificador notificador)
        {
            Notificador = notificador;
        }


        public IActionResult CustomResponse(object data = null)
        {
            if (!OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = data
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    erros = Notificador.ObterErros()
                });
            }


        }

        private bool OperacaoValida()
        {
            return Notificador.TemErro();
        }

    }

}
