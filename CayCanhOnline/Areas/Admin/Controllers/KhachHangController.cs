using CayCanhOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class KhachHangController : BaseController
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        public KHACHHANG GetKH(int id)
        {
            return (data.KHACHHANGs.OrderBy(n => n.HoTen).Where(n => n.MaKH == id).SingleOrDefault());
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(GetKH(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                var kh = GetKH(int.Parse(Request.Form["MaKH"]));
                kh.HoTen = Request.Form["HoTen"];
                kh.TaiKhoan = Request.Form["TaiKhoan"];
                kh.MatKhau = Request.Form["MatKhau"];
                kh.Email = Request.Form["Email"];
                kh.DiaChi = Request.Form["DiaChi"];
                kh.DienThoai = Request.Form["DienThoai"];
                kh.NgaySinh = Convert.ToDateTime(Request.Form["NgaySinh"]);
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
            return View(GetKH(id));
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection f)
        {
            var kh = GetKH(id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //var ctdh = data.CHITIETDATHANGs.Where(ct => ct.MaSach == id).ToList();
            //if (ctdh != null)
            //{
            //    ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng!";
            //    return RedirectToAction("Delete");
            //}
            data.KHACHHANGs.DeleteOnSubmit(kh);
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
        public ActionResult Create(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG kh = new KHACHHANG();
                //kh.MaKH = int.Parse(f["MaKH"]);
                kh.HoTen = Request.Form["HoTen"];
                kh.TaiKhoan = Request.Form["TaiKhoan"];
                kh.MatKhau = Request.Form["MatKhau"];
                kh.Email = Request.Form["Email"];
                kh.DiaChi = Request.Form["DiaChi"];
                kh.DienThoai = Request.Form["DienThoai"];
                kh.NgaySinh = Convert.ToDateTime(Request.Form["dNgaySinh"]);
                data.SubmitChanges();
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return Redirect("Index");
            }
            else
            {
                return RedirectToAction("Delete");
            }
        }
    }
}