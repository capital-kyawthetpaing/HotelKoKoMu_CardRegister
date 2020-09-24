using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class CardRegisterModel
    {
        public string GuestName { get; set; }
        public string GuestNameImage { get; set; }
        public string KanaName { get; set; }
        public string KanaNameImage { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeImage { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneNoImage { get; set; }
        public string Address1 { get; set; }
        public string AddressImage1 { get; set; }
        public string Address2 { get; set; }
        public string AddressImage2 { get; set; }
        public string WorkPlace { get; set; }
        public string WorkPlaceImage { get; set; }
        public string Nationality { get; set; }
        public string NationalityImage { get; set; }
        public string Passport { get; set; }
        public string PassportImage { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Sign { get; set; }
        public string SignImage { get; set; }


    }
}