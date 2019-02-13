using System;
using System.Web;

namespace ASPUnhandledException
{
    // '' <summary>
    // '' Implements ASP unhandled exception manager as a HttpModule
    // '' </summary>
    // '' <remarks>
    // '' to use:
    // ''    1) place ASPUnhandledException.dll in the \bin folder
    // ''    2) add this section to your Web.config under the <system.web> element:
    // ''         <httpModules>
    // ''                 <add name="ASPUnhandledException" 
    // ''                 type="ASPUnhandledException.UehHttpModule, ASPUnhandledException" />
    // ''            </httpModules>
    // ''
    // ''  Jeff Atwood
    // ''  http://www.codinghorror.com/
    // '' </remarks>
    public class UehHttpModule : IHttpModule
    {
        public void Init(HttpApplication Application)
        {
            //Application.Error += new System.EventHandler(this.OnError);
            Application.Error += OnError;
        }

        public void Dispose()
        {
        }

        protected virtual void OnError(object sender,
                                       EventArgs args)
        {
            var app = ((HttpApplication) (sender));
            var ueh = new Handler();

            ueh.HandleException(app.Server.GetLastError());
        }
    }
}
