using CayCanhOnline.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CayCanhOnline.Controllers
{
    public class CayCanhOnlineController : Controller
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();

        private List<SANPHAM> LaySPMoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        private List<SANPHAM> LaySPTieuCanhDeBan(int count)
        {
            return data.SANPHAMs.Where(a=> a.MaDM == 1).OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        private List<SUKIEN> LaySuKien(int count)
        {
            return data.SUKIENs.OrderByDescending(a => a.NgayDang).Take(count).ToList();
        }

        private List<DanhMucViewModel> LayDanhMuc(int count)
        {
            return (from DANHMUCs in data.DANHMUCs
                    join SANPHAMs in data.SANPHAMs on new { MaDM = DANHMUCs.MaDM } equals new { MaDM = Convert.ToInt32(SANPHAMs.MaDM) } into SANPHAMs_join
                    from SANPHAMs in SANPHAMs_join.DefaultIfEmpty()
                    group new { DANHMUCs, SANPHAMs } by new
                    {
                        DANHMUCs.MaDM,
                        DANHMUCs.TenDM,
                        DANHMUCs.AnhBia
                    } into g
                    orderby
                      g.Count(p => p.SANPHAMs.MaDM != null) descending
                    select new DanhMucViewModel
                    
                    {
                        MaDM = (int)g.Key.MaDM,
                        TenDM = g.Key.TenDM,
                        AnhBia = g.Key.AnhBia,
                        
                    }).Take(count).ToList();
        }

        // GET: CayCanhOnline
        public ActionResult Index()
        {
            var listSPMoi = LaySPMoi(5);
            return View(listSPMoi);
        }

        public ActionResult NavPartial()
        {
            var listDM = from dm in data.DANHMUCs select dm;
            return PartialView(listDM);
        }

        public ActionResult TieuCanhDeBanPartial()
        {
            var listSP = LaySPTieuCanhDeBan(5);
            return PartialView(listSP);
        }

        public ActionResult DanhMucPartial()
        {
            var listDM = LayDanhMuc(4);
            return PartialView(listDM);
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var sp = from s in data.SANPHAMs where s.MaSP == id select s;
            return View(sp.Single());
        }

        public ActionResult DanhMucChiTietPartial()
        {
            var listDM = from dm in data.DANHMUCs select dm;
            return PartialView(listDM);
        }

        public ActionResult SPTheoDM(int iMaDM, int? page)
        {
            ViewBag.MaDM = iMaDM;
            int iSize = 4;
            int iPageNum = (page ?? 1);
            var sp = from s in data.SANPHAMs where s.MaDM == iMaDM select s;
            return View(sp.ToPagedList(iPageNum, iSize));
            
        }

        public ActionResult SuKien()
        {
            var listSK = LaySuKien(3);
            return PartialView(listSK);
        }

        public ActionResult ChiTietSuKien(int id)
        {
            var sk = from s in data.SUKIENs where s.MaSK == id select s;
            return View(sk.Single());
        }

        public ActionResult About()
        {
            var about = from s in data.ABOUTs where s.id == 1 select s;
            return View(about.Single());
        }
    }
}