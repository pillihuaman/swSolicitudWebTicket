using System;
using System.Web;

//using CustomLog;

namespace swSolicitudes
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //// registrando evento
            //Bitacora.Current.Info<Global>("Iniciando SW Solicitudes.");
        }

        /*
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }
        */

        protected void Application_Error(object sender, EventArgs e)
        {
            var lex = Server.GetLastError();

            //// registrando evento
            //Bitacora.Current.Error<Global>(lex, null);

            Server.ClearError();
        }

        /*
        protected void Session_End(object sender, EventArgs e)
        {
        }
        */

        protected void Application_End(object sender, EventArgs e)
        {
            //// registrando evento
            //Bitacora.Current.Info<Global>("Deteniendo SW Solicitudes.");
        }
    }
}