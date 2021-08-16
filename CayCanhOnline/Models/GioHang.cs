using CayCanhOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CayCanhOnline.Models
{
    public class GioHang
    {
        dbCayCanhOnlineDataContext data = new dbCayCanhOnlineDataContext();
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public GioHang(int msp)
        {
            iMaSP = msp;
            SANPHAM s = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSP = s.TenSP;
            sAnhBia = s.AnhBia;
            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}