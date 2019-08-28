using System.Web.Mvc;
using System.Web.Routing;

namespace TafelFlyingServices
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute("Day", "{controller}/{action}/{year}/{month}/{day}", new
            {
                controller = "Flights",
                action = "Index",
                year = UrlParameter.Optional,
                month = UrlParameter.Optional,
                day = UrlParameter.Optional
            }
                );

            routes.MapRoute("Month", "{controller}/{action}/{year}/{month}", new
            {
                controller = "Flights",
                action = "Calendar",
                year = UrlParameter.Optional,
                month = UrlParameter.Optional
            }
                );
        }
    }
}