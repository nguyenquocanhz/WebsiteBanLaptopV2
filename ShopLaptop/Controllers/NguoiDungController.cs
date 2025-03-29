using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopLaptop.Models;
using ShopLaptop.Common;
using System.Globalization;

namespace ShopLaptop.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        MyDataDataContext data = new MyDataDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["soDienThoai"];
            var ngaysinh = collection["ngaySinh"];

            var response = new { success = false, message = "" };

            if (string.IsNullOrEmpty(MatKhauXacNhan))
            {
                return Json(new { success = false, message = "Phải nhập mật khẩu xác nhận!" });
            }

            if (matkhau != MatKhauXacNhan)
            {
                return Json(new { success = false, message = "Mật khẩu và mật khẩu xác nhận phải giống nhau!" });
            }

            try
            {
                kh.hoten = hoten;
                kh.tendangnhap = tendangnhap;
                kh.matkhau = CommonFields.getStringSHA256Hash(matkhau).Substring(0, 32);
                kh.email = email;
                kh.diachi = diachi;
                kh.dienthoai = dienthoai;
                DateTime parsedDate;
                if (!DateTime.TryParseExact(collection["ngaysinh"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return Json(new { success = false, message = "Ngày sinh không hợp lệ!" });
                }
                kh.ngaysinh = parsedDate;
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                Session["TaiKhoan"] = kh;

                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đăng ký thất bại! " + ex.Message });
            }
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DangNhap(string tendangnhap, string matkhau)
        {
            var hashedPassword = CommonFields.getStringSHA256Hash(matkhau).Substring(0, 32);
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == hashedPassword);

            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
                return Json(new { success = true, message = "Đăng nhập thành công !", redirectUrl = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng!" });
            }
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}