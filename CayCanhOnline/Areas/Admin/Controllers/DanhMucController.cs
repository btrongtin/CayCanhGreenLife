using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class DanhMucController : BaseController
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();

        // GET: DanhMuc
        public ActionResult Index()
        {
            return View(from dm in data.DANHMUCs select dm);
        }

        public ActionResult Details()
        {
            int madm = int.Parse(Request.QueryString["id"]);
            var result = data.DANHMUCs.Where(dm => dm.MaDM == madm).SingleOrDefault();
            return View(result);
        }

        public DANHMUC GetDM(int id)
        {
            return (data.DANHMUCs.OrderBy(n => n.TenDM).Where(n => n.MaDM == id).SingleOrDefault());
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //NHAXUATBAN nxb = data.NHAXUATBANs.Where(n => n.MaNXB == id).SingleOrDefault();
            //return View(nxb);
            return View(GetDM(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                var dm = GetDM(int.Parse(Request.Form["MaDM"]));
                dm.TenDM = Request.Form["TenDM"];
                dm.AnhBia = Request.Form["AnhBia"];

                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(GetDM(id));
        }

        [HttpPost/*, ActionName("Delete")*/]
        public ActionResult Delete/*Confirm*/(int id, FormCollection f)
        {
            var dm = GetDM(id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            data.DANHMUCs.DeleteOnSubmit(dm);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DANHMUC dm, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenDM = f["TenDM"];
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
                    dm.TenDM = f["TenDM"];
                    dm.AnhBia = sFileName;

                    data.DANHMUCs.InsertOnSubmit(dm);
                    data.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            //if (ModelState.IsValid)
            //{
            //    DANHMUC dm = new DANHMUC();
            //    //dm.MaNXB = int.Parse(Request.Form["MaNXB"]);
            //    dm.TenDM = Request.Form["TenDM"];
            //    dm.AnhBia = Request.Form["AnhBia"];

            //    data.SubmitChanges();
            //    //s.DonGia = Convert.ToDecimal(f["DonGia"]);
            //    //s.SLTon = Convert.ToDecimal(f["SLTon"]);
            //    data.DANHMUCs.InsertOnSubmit(dm);
            //    data.SubmitChanges();
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    return RedirectToAction("Delete");
            //}
        }
    }
}