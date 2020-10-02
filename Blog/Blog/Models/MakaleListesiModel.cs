using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class MakaleListesiModel
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Yazi { get; set; }
        public DateTime Tarih { get; set; }
        public string YazarAdi { get; set; }
        public string KategoriAdi { get; set; }
        public string resim_yol { get; set; }
        public int yazi_id { get; set; }
        public int Kid { get; set; }
        public int Yid { get; set; }
        public int resim_id { get; set; }
    }
}