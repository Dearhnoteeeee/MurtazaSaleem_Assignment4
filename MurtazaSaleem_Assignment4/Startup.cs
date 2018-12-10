using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MurtazaSaleem_Assignment4.Startup))]
namespace MurtazaSaleem_Assignment4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
