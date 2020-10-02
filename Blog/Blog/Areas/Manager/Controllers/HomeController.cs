using Blog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Manager.Controllers
{
    public class HomeController : Controller
    {
        bloggerEntities db = new bloggerEntities();
        [Authorize(Roles ="1,2,3")]
        public ActionResult Index()
        {
            var t = db.makale.Count();
            var y = db.yazar.Count();
            var k = db.kategori.Count();
            ViewBag.KategoriSayisi = k;
            ViewBag.YazarSayisi = y;
            ViewBag.MakaleSayisi = t;
            return View();
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult MakaleEkle()
        {
           
            ViewBag.YazarListesi = new SelectList(db.yazar.ToList(), "id", "adi");
            ViewBag.KategoriListesi = new SelectList(db.kategori.ToList(), "id", "adi");
            return View();
        }
        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public ActionResult MakaleEkle(String baslikk,String yazii,int Yid, int Kid, HttpPostedFileBase varsayilan)
        {
            
                makale model = new makale();
                resim res = new resim();

                model.makale1 = yazii;
                model.baslik = baslikk;
                model.yazar_id = Yid;
                model.kategori_id = Kid;
                model.tarih = DateTime.Now;
                string gu = Guid.NewGuid().ToString();
                string resimad = gu + Path.GetFileName(varsayilan.FileName);
                var url = Path.Combine(Server.MapPath("~/Image/" + gu + varsayilan.FileName));
                varsayilan.SaveAs(url);

                res.yazi_id = model.id;
                res.resim_yol = resimad;
                res.varsayilan = resimad;
                db.resim.Add(res);
                db.makale.Add(model);
                db.SaveChanges();
            

            
            return RedirectToAction("Makaleler","Home");
        }
        [Authorize(Roles = "1,2")]
        public ActionResult YazarEkle()
        {



            return View();
        }
        [Authorize(Roles ="1")]
        [HttpPost]
        public ActionResult YazarEkle(String Adi,String Soyad,String email,String adres,int telno,int yetki,string sifre)
        {
            yazar model = new yazar();

            model.adi = Adi;
            model.soyadi = Soyad;
            model.email = email;
            model.adres = adres;
            model.tel_no = telno;
            model.yetki = yetki;
            model.sifre = sifre;

            db.yazar.Add(model);
            db.SaveChanges();

            return RedirectToAction("Yazarlar", "Home");
        }
        [Authorize(Roles = "1,2")]
        public ActionResult KategoriEkle()
        {


            return View();
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult KategoriEkle(String kategorii)
        {
            kategori model = new kategori();

            model.adi = kategorii;

            db.kategori.Add(model);
            db.SaveChanges();

            return RedirectToAction("Kategoriler","Home");
        }
        [Authorize(Roles = "1,2")]
        public ActionResult ResimEkle()
        {


            return View();
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult ResimEkle(IEnumerable<HttpPostedFileBase> resim,int Id)
        {

            resim model = new resim();

            foreach (var item in resim)
            {
                string gu = Guid.NewGuid().ToString();
                string resimad = gu+Path.GetFileName(item.FileName);
                var url = Path.Combine(Server.MapPath("~/Image/"+gu + item.FileName));
                item.SaveAs(url);

                model.resim_yol = resimad;
                model.yazi_id = Id;

                db.resim.Add(model);
                db.SaveChanges();
            }
            return View();
        }


        [Authorize(Roles = "1,2,3")]
        public ActionResult Makaleler()
        {

           

             List<MakaleListesiModel> makaleler = new List<MakaleListesiModel>(); //boş bir liste oluşturduk

             List<makale> makalelist = db.makale.ToList();//makale tabllosundaki verileri listeye attık


             foreach (var item in makalelist)
             {
                 MakaleListesiModel m = new MakaleListesiModel();
                 yazar y = db.yazar.FirstOrDefault(x => x.id == item.yazar_id);
                 kategori c = db.kategori.FirstOrDefault(z => z.id == item.kategori_id);
                 m.Id = item.id;
                 m.Baslik = item.baslik;
                 m.Yazi = item.makale1;
                 m.Tarih = item.tarih.Value;
                 m.YazarAdi =y.adi;
                 m.KategoriAdi = c.adi;


                 makaleler.Add(m);

                     }

            return View(makaleler);
        }
        [Authorize(Roles = "1")]
        public ActionResult Yazarlar()
        {

            List<yazar> yazarlar = db.yazar.ToList();

            return View(yazarlar);
        }
        [Authorize(Roles = "1,2")]
        public ActionResult Kategoriler()
        {
            List<kategori> kategoriler = db.kategori.ToList();

            return View(kategoriler);
        }
        [Authorize(Roles ="1")]

        public ActionResult MakaleSil(int Id)
        {
            var makale = db.makale.Where(x => x.id == Id).FirstOrDefault();
            

            
            db.makale.Remove(makale);
            
            db.SaveChanges();

            return RedirectToAction("Makaleler", "Home");
        }
        [Authorize(Roles ="1")]
        public ActionResult KategoriSil(int Id)
        {

            var kategori = db.kategori.Where(x => x.id == Id).FirstOrDefault();

            db.kategori.Remove(kategori);
            db.SaveChanges();

            return RedirectToAction("Kategoriler","Home");
        }
        [Authorize(Roles ="1")]
        public ActionResult YazarSil(int id)
        {

            var yazar = db.yazar.Where(x => x.id == id).FirstOrDefault();

            db.yazar.Remove(yazar);
            db.SaveChanges();

            return RedirectToAction("Yazarlar", "Home");
        }
        [Authorize(Roles = "1,2,3")]
        public ActionResult MakaleDuzenle(int id)
        {
     

            makale makaleler = db.makale.Where(x => x.id == id).FirstOrDefault();
            MakaleListesiModel model = new MakaleListesiModel();
            model.Id = makaleler.id;
            model.Yid = makaleler.yazar_id.Value;
            model.Kid = makaleler.kategori_id.Value;
            ViewBag.YazarListesi = new SelectList(db.yazar.ToList(), "id", "adi", makaleler.yazar_id);
            ViewBag.KategoriListesi = new SelectList(db.kategori.ToList(), "id", "adi", makaleler.kategori_id);
            model.Baslik = makaleler.baslik;
            
            model.Yazi = makaleler.makale1;
            
            return View(model);
        }
        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public ActionResult MakaleDuzenle(MakaleListesiModel mak)
        {
            
           
            makale i = db.makale.FirstOrDefault(x => x.id == mak.Id);
            i.baslik = mak.Baslik;
            i.makale1 = mak.Yazi;
            i.yazar_id = mak.Yid;
            i.kategori_id = mak.Kid;
            db.SaveChanges();
            return RedirectToAction("Makaleler","Home");
        }
        [Authorize(Roles = "1,2")]
        public ActionResult KategoriDuzenle(int id)
        {
            kategori kategoriler = db.kategori.Where(x => x.id == id).FirstOrDefault();
            kategori model = new kategori();
            model.id = kategoriler.id;
            model.adi = kategoriler.adi;

            return View(model);
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public ActionResult KategoriDuzenle(MakaleListesiModel kat)
        {
            kategori i = db.kategori.Where(X => X.id == kat.Id).FirstOrDefault();
            i.adi = kat.KategoriAdi ;

            db.SaveChanges();


            return RedirectToAction("Kategoriler", "Home");
        }
        [Authorize(Roles ="1")]
        public ActionResult YazarDuzenle(int id)
        {
            yazar yazarlar = db.yazar.Where(x => x.id == id).FirstOrDefault();
            yazar model = new yazar();
            model.id = yazarlar.id;
            model.adi = yazarlar.adi;
            model.soyadi = yazarlar.soyadi;
            model.email = yazarlar.email;
            model.adres = yazarlar.adres;
            model.tel_no = yazarlar.tel_no;
            model.yetki = yazarlar.yetki;
            model.sifre = yazarlar.sifre;

            return View(model);
        }
        [Authorize(Roles ="1")]
        [HttpPost]

        public ActionResult YazarDuzenle(YazarBilgileri yaz)
        {
            yazar i = db.yazar.Where(X => X.id == yaz.id).FirstOrDefault();
            i.adi = yaz.adi;
            i.soyadi = yaz.soyadi;
            i.email = yaz.email;
            i.adres = yaz.adres;
            i.tel_no = yaz.tel_no;
            i.yetki = yaz.yetki;
            i.sifre = yaz.sifre;


            db.SaveChanges();

            return RedirectToAction("Yazarlar", "Home");
        }
    }
}