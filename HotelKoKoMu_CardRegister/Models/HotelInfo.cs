using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class HotelInfo:BaseInfo
    {
        public string HotelNo { get; set; }
        public string HotelName { get; set; }
        public string LogoData { get; set; }
    }

}