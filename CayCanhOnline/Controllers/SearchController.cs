using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Controllers
{
    public class SearchController : Controller
    {
        dbCayCanhOnlineDataContext db = new dbCayCanhOnlineDataContext();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {

                //var kq = from s in db.SACHes where s.MaCD == int.Parse(strSearch) select s;
                //var kq = db.SACHes.Where(s => s.MaCD == int.Parse(strSearch)).OrderByDescending(s => s.SoLuongBan);
                var kq = from s in db.SANPHAMs where s.TenSP.Contains(strSearch) select s;
                //var kq = from s in db.SACHes where s.SoLuongBan >= 5 && s.SoLuongBan <= 10 select s;
                //var kq = from s in db.SANPHAMs
                //         where s.TenSP.Contains(strSearch)
                //         || s.MaSP == int.Parse(strSearch) || s.MoTa.Contains(strSearch)

                //         select s;
                //var kq = from s in db.SACHes where (s.SoLuongBan >= 5 && s.SoLuongBan <= 10) orderby s.SoLuongBan ascending select s;
                //var kq = from s in db.SACHes where (s.MaCD == int.Parse(strSearch)) orderby s.SoLuongBan descending select s;
                return View(kq);
            }
            return View();
        }
    }
}