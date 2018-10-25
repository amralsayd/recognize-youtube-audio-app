using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YoutubeTool.Presentaion.Website.Startup))]
namespace YoutubeTool.Presentaion.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
