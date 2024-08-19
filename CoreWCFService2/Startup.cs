using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using CoreWCF;
using CoreWCF.Configuration;

[assembly: OwinStartup(typeof(CoreWCFService2.Startup))]
namespace CoreWCFService2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            app.MapSignalR();

          
        }
    }
}
