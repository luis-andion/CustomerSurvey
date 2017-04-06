using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomerSurveyWeb.Startup))]
namespace CustomerSurveyWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
