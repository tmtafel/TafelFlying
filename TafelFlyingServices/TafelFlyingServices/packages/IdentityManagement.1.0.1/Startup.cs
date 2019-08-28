using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Commerce.IdentityManagement.Startup))]
namespace Commerce.IdentityManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
