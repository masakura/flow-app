using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FlowApp.Startup))]
namespace FlowApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
