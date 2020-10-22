using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e-RegistrationCardSystem.Models
{
    public class CardRegisterInfo
    {
        public string SystemID { get; set; }
        public string PmsID { get; set; }        
        public string PmsPassword { get; set; }
        public string HotelCode { get; set; }
        public string MachineNo { get; set; }        
        public string ReservationNo { get; set; }
        public string RoomNo { get; set; }
        public string SystemDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NameKanji { get; set; }
        public string NameKana { get; set; }       
        public string ZipCode { get; set; }       
        public string Tel { get; set; }       
        public string Address1 { get; set; }       
        public string Address2 { get; set; }        
        public string Company { get; set; }        
        public string Nationality { get; set; }       
        public string PassportNo { get; set; }        
        public string ArriveDate { get; set; }
        public string DepartureDate { get; set; }
        public string Sign { get; set; }
        public string ImageData { get; set; }
        public string Creator { get; set; }
        public string Updator { get; set; }
        public string json { get; set; }
    }
}