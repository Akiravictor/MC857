using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LojaVirtual.Startup))]
namespace LojaVirtual
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
