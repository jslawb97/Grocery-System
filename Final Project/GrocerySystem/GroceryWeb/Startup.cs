using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroceryWeb.Startup))]
namespace GroceryWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
