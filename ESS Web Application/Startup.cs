using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ESS_Web_Application.Startup))]
namespace ESS_Web_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
