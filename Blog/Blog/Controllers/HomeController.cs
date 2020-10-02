using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        bloggerEntities db = new bloggerEntities();
 
        public ActionResult Index()
        {
            List<MakaleListesiModel> kmakale = new List<MakaleListesiModel>();
            List<makale> veri = db.makale.ToList();

            foreach (var item in veri)
            {
                MakaleListesiModel m = new MakaleListesiModel();
                item.makale1 = Regex.Replace(item.makale1, @"<(.\n)*?>", string.Empty);

                if (item.makale1.Length > 120) item.makale1 = item.makale1.Substring(0, 120);

                item.makale1 = item.makale1 + "...";
                m.Id = item.id;
                m.Baslik = item.baslik;
                m.resim_yol = db.resim.FirstOrDefault(x => x.yazi_id == item.id).resim_yol;
                m.Yazi = item.makale1;
                m.Tarih = item.tarih.Value;

                kmakale.Add(m);
            }
           
            return View(kmakale);
        }

        public ActionResult MakaleDetay(int id)
        {

            MakaleListesiModel model = new MakaleListesiModel();
            makale mak = db.makale.FirstOrDefault(x=> x.id == id);
            yazar yaz = db.yazar.FirstOrDefault(x => x.id == mak.yazar_id);
           

            model.Baslik = mak.baslik;
            model.resim_yol = db.resim.FirstOrDefault(x => x.yazi_id == mak.id).resim_yol;
            model.Yazi = mak.makale1;
            model.Tarih = mak.tarih.Value;
            model.YazarAdi = yaz.adi;


            return View(model);
        }

      
    }
}