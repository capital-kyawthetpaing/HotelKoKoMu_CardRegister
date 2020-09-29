using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelKoKoMu_CardRegister.Models
{
    public class HotelSystem:BaseModel
    {

        public string System_ID { get; set; }

        public string PMS_ID { get; set; }

        public string PMS_Password { get; set; }

        public string Hotel_Code { get; set; }

        public string Machine_No { get; set; }

        public string Reservation_No { get; set; }

        public string Room_No { get; set; }

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

        public string System_Date { get; set; }





    }
}