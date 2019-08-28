using Microsoft.Owin;
using Owin;
using TafelFlyingServices;

[assembly: OwinStartup(typeof (Startup))]

namespace TafelFlyingServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}