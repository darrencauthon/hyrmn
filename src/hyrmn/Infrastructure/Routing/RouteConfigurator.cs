using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace hyrmn.Infrastructure.Routing
{
    public class RouteConfigurator : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.IgnoreRoute("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });

            AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
