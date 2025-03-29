using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopLaptop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Route tùy chỉnh cho trang đăng nhập
            routes.MapRoute(
                name: "DangNhap",
                url: "auth/login",
                defaults: new { controller = "NguoiDung", action = "DangNhap" }
            );
            routes.MapRoute(
                name: "DangKy",
                url: "auth/register",
                defaults: new { controller = "NguoiDung", action = "DangKy" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
