using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1291163_MVC_EF_Project.Startup))]
namespace _1291163_MVC_EF_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
