using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebInvoicing.Startup))]
namespace WebInvoicing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
