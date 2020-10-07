using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class CardRegisterInfo:BaseInfo
    {
        public string PmsID { get; set; }
        public string SystemID { get; set; }
        public string PmsPassword { get; set; }
        public string MachineNo { get; set; }
        public string HotelCode { get; set; }
        public string ReservationNo { get; set; }
        public string RoomNo { get; set; }
        public DateTime SystemDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string GuestName { get; set; }
        public string GuestNameHW { get; set; }
        public string KanaName { get; set; }
        public string KanaNameHW { get; set; }
        public string ZipCode { get; set; }
        public string ZipCodeHW { get; set; }
        public string Tel { get; set; }
        public string TelHW { get; set; }
        public string Address1 { get; set; }
        public string AddressHW1 { get; set; }
        public string Address2 { get; set; }
        public string AddressHW2 { get; set; }
        public string Company { get; set; }
        public string CompanyHW { get; set; }
        public string Nationality { get; set; }
        public string NationalityHW { get; set; }
        public string Passport { get; set; }
        public string PassportHW { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Sign { get; set; }
        public string ImageData { get; set; }
       
    }
}