using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_Management_App.Startup))]
namespace Project_Management_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
