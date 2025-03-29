using System.Web.Mvc;

namespace ShopLaptop.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Administrator"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Định tuyến trang chính của Admin
            context.MapRoute(
                name: "Admin_Home",
                url: "Admin",
                defaults: new { controller = "MainPage", action = "Index" },
                namespaces: new[] { "ShopLaptop.Areas.Administrator.Controllers" }
            );

            // Định tuyến trang đăng nhập của Admin
            context.MapRoute(
                name: "Admin_Login",
                url: "Admin/Login",
                defaults: new { controller = "MainPage", action = "Login" },
                namespaces: new[] { "ShopLaptop.Areas.Administrator.Controllers" }
            );
            // định tuyến api auth đăng nhập admin
            context.MapRoute(
                name: "AuthAdmin_Login",
                url: "auth/admin/login",
                defaults: new { controller = "MainPage", action = "Login" },
                namespaces: new[] { "ShopLaptop.Areas.Administrator.Controllers" }
            );
            // Định tuyến mặc định cho khu vực Administrator
            context.MapRoute(
                name: "Administrator_Default",
                url: "Administrator/{controller}/{action}/{id}",
                defaults: new { controller = "MainPage", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ShopLaptop.Areas.Administrator.Controllers" }
            );
        }
    }
}
