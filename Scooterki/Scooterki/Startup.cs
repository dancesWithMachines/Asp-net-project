using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Scooterki.Startup))]
namespace Scooterki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        
    }
}
