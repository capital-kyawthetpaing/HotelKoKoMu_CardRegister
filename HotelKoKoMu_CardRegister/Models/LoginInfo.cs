using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class LoginInfo: BaseInfo
    {
        public string SystemID { get; set; }
        public string PmsID { get; set; }
        public string PmsPassword { get; set; }
        public string MachineNo { get; set; }
        public string HotelCode { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}