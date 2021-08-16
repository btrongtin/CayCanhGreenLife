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
    public class SanPhamController : BaseController
    {
        dbCayCanhOnlineDataContext db = new dbCayCanhOnlineDataContext();

        // GET: Admin/SanPham
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sp, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenSP = f["sTenSP"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", int.Parse(f["MaDM"]));
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
                    sp.TenSP = f["sTenSP"];
                    sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    sp.AnhBia = sFileName;
                    sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sp.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sp.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sp.MaDM = int.Parse(f["MaDM"]);
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
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
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
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
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSP == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sản phẩm này đang có trong bảng chi tiết đặt hàng <br>" +
                "Nếu muốn xóa thì phải xóa hết mã sản phẩm này trong bảng chi tiết đặt hàng";
                return View(sp);
            }

            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", sp.MaDM);

            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == int.Parse(f["iMaSP"]));
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", sp.MaDM);

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
                sp.TenSP = f["sTenSP"];
                sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");

                sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sp.SoLuongBan = int.Parse(f["iSoLuong"]);
                sp.GiaBan = decimal.Parse(f["mGiaBan"]);
                sp.MaDM = int.Parse(f["MaDM"]);

                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
    }
}