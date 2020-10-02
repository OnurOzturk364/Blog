using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class YazarBilgileri
    {
        public int id { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public string email { get; set; }
        public string adres { get; set; }
        public int tel_no { get; set; }
        public int yetki { get; set; }
        public string sifre { get; set; }
    }
}