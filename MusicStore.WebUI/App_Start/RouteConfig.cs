using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                   null,
                   "",
                   new { controller = "Home", action = "Index", page = 1 }
               );

            routes.MapRoute(
                  null,
                  "Strona{page}",
                  new { controller = "Home", action = "Index" },
                  new {page =@"\d+"}
             );

              

            routes.MapRoute(null, "{genre}", new
            {
                controller = "Home",
                action = "FiltrByGenre",
                page = 1
            });

            routes.MapRoute(null, "{genre}/Strona{page}", new { controller = "Home", action = "FiltrByGenre" }, new { page = @"\d+" });

            routes.MapRoute(null, "{controller}/{action}");

            routes.MapRoute(
                null,
                "{controller}/{action}/{id}",
                new { controller = "Admin", action = "DeleteAlbum", id = UrlParameter.Optional }
                );
        }
    }
}
