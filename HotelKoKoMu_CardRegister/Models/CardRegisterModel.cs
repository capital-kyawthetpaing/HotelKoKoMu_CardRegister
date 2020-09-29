using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class CardRegisterModel:BaseModel
    {
        public string HotelCode { get; set; }
        public string ReservationNo { get; set; }
        public string RoomNo { get; set; }
        public DateTime SystemDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GuestName { get; set; }
        public string GuestNameHW { get; set; }
        public string KanaName { get; set; }
        public string KanaNameHW { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeHW { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneNoHW { get; set; }
        public string Address1 { get; set; }
        public string AddressHW1 { get; set; }
        public string Address2 { get; set; }
        public string AddressHW2 { get; set; }
        public string WorkPlace { get; set; }
        public string WorkPlaceHW { get; set; }
        public string Nationality { get; set; }
        public string NationalityHW { get; set; }
        public string Passport { get; set; }
        public string PassportHW { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Sign { get; set; }
    }
}