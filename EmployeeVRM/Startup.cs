using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeVRM.Startup))]
namespace EmployeeVRM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
