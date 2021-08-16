using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class DonHangController : BaseController
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();
        public ActionResult Index()
        {
            return View(from dh in data.DONDATHANGs select dh);
        }
        public ActionResult Details(int id)
        {
            var dh = data.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }
        public DONDATHANG GetDH(int id)
        {
            return data.DONDATHANGs.OrderBy(n => n.KHACHHANG.HoTen).Where(n => n.MaDonHang == id).SingleOrDefault();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(GetDH(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                var dh = GetDH(int.Parse(Request.Form["MaDonHang"]));
                dh.DaThanhToan = bool.Parse(Request.Form["DaThanhToan"]);
                dh.TinhTrangGiaoHang = int.Parse(Request.Form["TinhTrangGiaoHang"]);
                dh.NgayDat = Convert.ToDateTime(Request.Form["NgayDat"]);
                dh.NgayGiao = Convert.ToDateTime(Request.Form["NgayGiao"]);
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
            return View(GetDH(id));
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection f)
        {
            var dh = GetDH(id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.DONDATHANGs.DeleteOnSubmit(dh);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}