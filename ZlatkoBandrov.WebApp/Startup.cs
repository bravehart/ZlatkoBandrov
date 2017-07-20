using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZlatkoBandrov.WebApp.Startup))]
namespace ZlatkoBandrov.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
