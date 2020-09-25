using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class CardRegisterModel:BaseModel
    {
        public string Mode { get; set; }
        public string GuestNo { get; set; }
        public string GuestName { get; set; }
        public string KanaName { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string WorkPlace { get; set; }
        public string Nationality { get; set; }
        public string Passport { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Sign { get; set; }
    }
}