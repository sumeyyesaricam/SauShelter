using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SauShelter.Startup))]
namespace SauShelter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
