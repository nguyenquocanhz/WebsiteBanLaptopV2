using Newtonsoft.Json;
using ShopLaptop.Common;
using ShopLaptop.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Areas.Administrator.Controllers
{
    public class MainPageController : Controller
    {
        // GET: Administrator/MainPage
        DataModel db = new DataModel();
        // GET: Admin/adm_MainPage
        public ActionResult Index()
        {
            if (Session["taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "MainPage");
            }
            else
            {
                return View();
            }            
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(FormCollection collection)
        {
            var taikhoan = collection["username"];
            var matkhau = collection["password"];
            var passwordHash = Encryptor.MD5Hash(matkhau).Substring(0, 32);

            if (string.IsNullOrEmpty(taikhoan) || string.IsNullOrEmpty(matkhau))
            {
                return Json(new { success = false, message = "Thông tin đăng nhập đang trống" });
            }

            Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == taikhoan && n.PassAdmin == passwordHash);
            if (ad != null)
            {
                Session["taikhoanadmin"] = ad;
                return Json(new { success = true, redirectUrl = Url.Action("Index", "MainPage") });
            }

            return Json(new { success = false, message = "Thông tin đăng nhập không đúng" });
        }


        public ActionResult Logout()
        {
            Session["taikhoanadmin"] = null;
            return RedirectToAction("Login", "MainPage");
        }
    }
}