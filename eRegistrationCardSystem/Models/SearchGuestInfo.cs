using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRegistrationCardSystem.Models
{
    public class SearchGuestInfo
    {
        public string HotelCode { get; set; }
        public DateTime ArrivalFromDate { get; set; }
        public DateTime ArrivalToDate { get; set; }
        public string RoomNo { get; set; }
        public string GuestName { get; set; }
    }
}