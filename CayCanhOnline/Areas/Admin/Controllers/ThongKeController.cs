using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class ThongKeController : BaseController
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();

        public ActionResult Thongke()
        {
            var kq = from s in data.SANPHAMs
                     join dm in data.DANHMUCs on s.MaDM equals dm.MaDM
                     group s by s.MaDM into g
                     select new ReportInfo
                     {

                         Id = g.Key.ToString(),
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan),
                         Avg = Convert.ToDecimal(g.Average(n => n.SoLuongBan))
                     };
            return PartialView(kq);
        }

        public ActionResult Group()
        {
            //var kq = from s in db.SACHes group s by s.MaCD;
            var kq = data.SANPHAMs.GroupBy(s => s.MaDM);
            return View(kq);
        }
    }
}