using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BankOrderSys.Startup))]
namespace BankOrderSys
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
