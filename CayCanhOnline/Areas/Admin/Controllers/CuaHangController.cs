using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class CuaHangController : BaseController
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();

        public ABOUT GetAbout(int id)
        {
            return (data.ABOUTs.OrderBy(n => n.id).Where(n => n.id == id).SingleOrDefault());
        }

        public ActionResult Details()
        {
            var result = data.ABOUTs.Where(dm => dm.id == 1).SingleOrDefault();
            return View(result);
        }

        public ActionResult Edit(int id)
        {
            return View(data.ABOUTs.Where(n => n.id == 1).SingleOrDefault());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var about = GetAbout(1);


            if (ModelState.IsValid)
            {

                about.TieuDe = f["sTieuDe"];
                about.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");

                about.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);


                data.SubmitChanges();
                return RedirectToAction("Details");
            }
            return View(about);
        }
    }
}