using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopLaptop.EF;

namespace ShopLaptop.Areas.Administrator.Controllers
{
    public class LaptopsController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Administrator/Laptops
        public ActionResult Index()
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                var laptops = db.Laptops.Include(l => l.Hang).Include(l => l.NhuCau);
                return View(laptops.ToList());
            }
        }

        // GET: Administrator/Laptops/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Laptop laptop = db.Laptops.Find(id);
                if (laptop == null)
                {
                    return HttpNotFound();
                }
                return View(laptop);
            }
        }

        // GET: Administrator/Laptops/Create
        public ActionResult Create()
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                ViewBag.mahang = new SelectList(db.Hangs, "mahang", "tenhang");
                ViewBag.manhucau = new SelectList(db.NhuCaus, "manhucau", "tennhucau");
                return View();
            }
        }

        // POST: Administrator/Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "malaptop,tenlaptop,giaban,mota,hinh,mahang,manhucau,cpu,gpu,ram,hardware,manhinh,ngaycapnhat,soluongton,pin,trangthai")] Laptop laptop)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                if (ModelState.IsValid)
                {
                    db.Laptops.Add(laptop);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.mahang = new SelectList(db.Hangs, "mahang", "tenhang", laptop.mahang);
                ViewBag.manhucau = new SelectList(db.NhuCaus, "manhucau", "tennhucau", laptop.manhucau);
                return View(laptop);
            }
        }

        // GET: Administrator/Laptops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Laptop laptop = db.Laptops.Find(id);
                if (laptop == null)
                {
                    return HttpNotFound();
                }
                ViewBag.mahang = new SelectList(db.Hangs, "mahang", "tenhang", laptop.mahang);
                ViewBag.manhucau = new SelectList(db.NhuCaus, "manhucau", "tennhucau", laptop.manhucau);
                return View(laptop);
            }
        }

        // POST: Administrator/Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "malaptop,tenlaptop,giaban,mota,hinh,mahang,manhucau,cpu,gpu,ram,hardware,manhinh,ngaycapnhat,soluongton,pin,trangthai")] Laptop laptop)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(laptop).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.mahang = new SelectList(db.Hangs, "mahang", "tenhang", laptop.mahang);
                ViewBag.manhucau = new SelectList(db.NhuCaus, "manhucau", "tennhucau", laptop.manhucau);
                return View(laptop);
            }
        }

        // GET: Administrator/Laptops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Laptop laptop = db.Laptops.Find(id);
                if (laptop == null)
                {
                    return HttpNotFound();
                }
                return View(laptop);
            }
        }

        // POST: Administrator/Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["taikhoanadmin"] == null)
                return RedirectToAction("Login", "MainPage");
            else
            {
                Laptop laptop = db.Laptops.Find(id);
                db.Laptops.Remove(laptop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn file!" });
            }

            // Kiểm tra định dạng file (chỉ chấp nhận JPG, PNG, WEBP)
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
            string fileExt = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExt))
            {
                return Json(new { success = false, message = "Error: Định dạng file không hợp lệ!" });
            }

            // Đổi tên file tránh trùng lặp
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string newFileName = fileName + "_" + DateTime.Now.Ticks + fileExt;
            string path = Server.MapPath("~/Content/images/" + newFileName);

            // Lưu file vào server
            file.SaveAs(path);

            // Trả về đường dẫn ảnh để hiển thị trên giao diện
            string imageUrl = "/Content/images/" + newFileName;    
            return Json(new { success = true, imageUrl });
        }


    }
}
