using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        dbCayCanhOnlineDataContext db = new dbCayCanhOnlineDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if(Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        
        public ActionResult LoginCheckPartial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["TenDN"];
            var sMatKhau = f["MatKhau"];
            //ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            //if (ad != null)
            //{
            //    Session["Admin"] = ad;
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";

            //}
            //return View();


            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa đăng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải đăng nhập mật khẩu";
            }
            else
            {
                ADMIN kh = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == GetMD5(sMatKhau));
                if (kh != null)
                {
                    //ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Admin"] = kh;
                    if (f["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return this.View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
        public ActionResult LoginAdminPartial()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login", "Home");
        }
    }
}