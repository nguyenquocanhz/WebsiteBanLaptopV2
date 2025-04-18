﻿using ShopLaptop.Common;
using ShopLaptop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLaptop.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.Find(n => n.malaptop == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        // EndPoint ThemGioHangAjax
        [HttpPost]
        public JsonResult ThemGioHangAjax(int id)
        {
            try
            {
                List<GioHang> lstGioHang = Laygiohang();
                GioHang sanpham = lstGioHang.Find(n => n.malaptop == id);

                if (sanpham == null)
                {
                    sanpham = new GioHang(id);
                    lstGioHang.Add(sanpham);
                }
                else
                {
                    sanpham.iSoluong++;
                }

                return Json(new
                {
                    success = true,
                    message = "Sản phẩm đã được thêm vào giỏ!",
                    tongSoLuong = TongSoLuong(),
                    tongTien = TongTien()
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoluong);
            }
            return tsl;

        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhTien);
            }
            return tt;

        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.malaptop == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.malaptop == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.malaptop == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Laptop s = new Laptop();
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            /*if ((bool)Session["thanhtoan"] == true)
            {
                dh.thanhtoan = true;
            }
            else
            {
                dh.thanhtoan = false;
            }*/


            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.malaptop = item.malaptop;
                ctdh.soluong = item.iSoluong;
                ctdh.dongia = (decimal)item.giaban;
                s = data.Laptops.Single(n => n.malaptop == item.malaptop);
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        [HttpPost]
        public JsonResult DatHangAjax(string ngayGiao)
        {
            try
            {
                if (Session["TaiKhoan"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để đặt hàng!" });
                }
                if (Session["GioHang"] == null)
                {
                    return Json(new { success = false, message = "Giỏ hàng trống, vui lòng thêm sản phẩm!" });
                }

                DonHang dh = new DonHang();
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                List<GioHang> gh = Laygiohang();

                dh.makh = kh.makh;
                dh.ngaydat = DateTime.Now;
                dh.ngaygiao = DateTime.Parse(ngayGiao);
                dh.giaohang = false;
                dh.thanhtoan = false;

                data.DonHangs.InsertOnSubmit(dh);
                data.SubmitChanges();

                // Tạo thông báo chi tiết về đơn hàng
                string orderDetails = $"Đơn hàng mới:\n" +
                                      $"- Mã đơn hàng: {dh.madon}\n" +
                                      $"- Ngày đặt: {dh.ngaydat:dd/MM/yyyy}\n" +
                                      $"- Ngày giao: {dh.ngaygiao:dd/MM/yyyy}\n" +
                                      $"- Khách hàng: {kh.hoten}\n" +
                                      $"- Danh sách sản phẩm:\n";

                foreach (var item in gh)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang
                    {
                        madon = dh.madon,
                        malaptop = item.malaptop,
                        soluong = item.iSoluong,
                        dongia = (decimal)item.giaban
                    };
                    data.ChiTietDonHangs.InsertOnSubmit(ctdh);

                    // Thêm sản phẩm vào chi tiết thông báo
                    orderDetails += $"\t- {item.tenlaptop} (SL: {item.iSoluong}, Giá: {item.giaban:N0})\n";
                }
                data.SubmitChanges();

                // Gửi thông báo đến Telegram
                try
                {
                    TelegramHelpers telegramHelpers = new TelegramHelpers();
                    telegramHelpers.SendOrderToTelegram(orderDetails);
                }
                catch (Exception ex)
                {
                    // Log lỗi gửi thông báo để theo dõi, không làm gián đoạn quy trình
                    Console.WriteLine($"Lỗi khi gửi thông báo Telegram: {ex.Message}");
                }

                Session["GioHang"] = null;

                return Json(new { success = true, message = "Đặt hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}