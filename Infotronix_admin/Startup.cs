using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Infotronix_admin.Startup))]
namespace Infotronix_admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
