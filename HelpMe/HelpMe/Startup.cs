using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelpMe.Startup))]
namespace HelpMe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
