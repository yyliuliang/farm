using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoldenFarm.Web.Startup))]
namespace GoldenFarm.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
