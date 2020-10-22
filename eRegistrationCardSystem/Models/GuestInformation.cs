using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRegistrationCardSystem.Models
{
    public class GuestInformation
    {
        public string SystemDate { get; set; }
        public string ReservationNo { get; set; }
        public string RoomNo { get; set; }
        public string NameKanji { get; set; }
        public string NameKana { get; set; }
        public string ZipCode { get; set; }
        public string Tel { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        public string PassportNo { get; set; }
        public string ImageData { get; set; }
        public string Status { get; set; }
        public string FailureReason { get; set; }
        public string ErrorDescription { get; set; }
    }
}