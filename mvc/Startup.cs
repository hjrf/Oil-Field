using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mvc.Startup))]
namespace mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
