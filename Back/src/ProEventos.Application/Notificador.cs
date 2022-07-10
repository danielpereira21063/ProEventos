using System.Collections.Generic;

namespace ProEventos.Application
{
    public class Notificador : INotificador
    {
        private static List<string> Erros;

        public void Notificar(string msg)
        {
            if (Erros == null)
            {
                Erros = new List<string>();
            }

            Erros.Add(msg);
        }

        public List<string> ObterErros()
        {
            return Erros;
        }

        public bool TemErro()
        {
            return (Erros?.Count ?? 0) > 0;
        }
    }
}

public interface INotificador
{

    public void Notificar(string msg);

    public List<string> ObterErros();

    public bool TemErro();
}
