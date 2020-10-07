using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class SearchGuestInfo
    {
        public DateTime ArrivalFromDate { get; set; }
        public DateTime ArrivalToDate { get; set; }
        public string RoomNo { get; set; }
        public string GuestName { get; set; }
    }
}