using System.Web.Http;

namespace WFE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Register",
                routeTemplate: "register",
                defaults: new { controller = "Chat", action = "Register" }
            );

            config.Routes.MapHttpRoute(
                name: "Send",
                routeTemplate: "send",
                defaults: new { controller = "Chat", action = "Send" }
            );

            config.Routes.MapHttpRoute(
                name: "SendFace",
                routeTemplate: "face/{from}/{to}",
                defaults: new { controller = "Chat", action = "SendFace" }
            );
        }
    }
}
