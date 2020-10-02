using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blog.Areas.Manager.Controllers
{
    public class GuvenlikController : Controller
    {
        bloggerEntities db = new bloggerEntities();
        // GET: Manager/Guvenlik
        public ActionResult GirisYap()
        {


            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(yazar y)
        {
            var bilgiler = db.yazar.FirstOrDefault(x => x.email == y.email && x.sifre == y.sifre);
            if (bilgiler !=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.email,false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

            
        }
    }
}