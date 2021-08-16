using CayCanhOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class SuKienController : Controller
    {
        dbCayCanhOnlineDataContext db = new dbCayCanhOnlineDataContext();

        // GET: Admin/SanPham
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.SUKIENs.ToList().OrderBy(n => n.NgayDang).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SUKIEN sp, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa."; 
                
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sp.TieuDe = f["sTieuDe"];
                    sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    sp.AnhBia = sFileName;
                    sp.NgayDang = Convert.ToDateTime(f["dNgayDang"]);
                    sp.NgayBatDau = Convert.ToDateTime(f["dNgayBatDau"]);
                    sp.NgayKetThuc = Convert.ToDateTime(f["dNgayKetThuc"]);
                    db.SUKIENs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var sp = db.SUKIENs.SingleOrDefault(n => n.MaSK == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sp = db.SUKIENs.SingleOrDefault(n => n.MaSK == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sp = db.SUKIENs.SingleOrDefault(n => n.MaSK == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            db.SUKIENs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sp = db.SUKIENs.SingleOrDefault(n => n.MaSK == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sp = db.SUKIENs.SingleOrDefault(n => n.MaSK == int.Parse(f["iMaSK"]));


            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);

                    }
                    sp.AnhBia = sFileName;
                }
                sp.TieuDe = f["sTieuDe"];
                sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");

                sp.NgayDang = Convert.ToDateTime(f["dNgayDang"]);
                sp.NgayBatDau = Convert.ToDateTime(f["dNgayBatDau"]);
                sp.NgayKetThuc = Convert.ToDateTime(f["dNgayKetThuc"]);

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
    }

}