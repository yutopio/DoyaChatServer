using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WFE
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.Formatters.Insert(0, new TextMediaTypeFormatter());
        }
    }
}
