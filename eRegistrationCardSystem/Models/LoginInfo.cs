﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRegistrationCardSystem.Models
{
    public class LoginInfo
    {
        public string SystemID { get; set; }
        public string PmsID { get; set; }
        public string PmsPassword { get; set; }
        public string MachineNo { get; set; }
        public string HotelCode { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public bool SessionFlag { get; set; }
    }

}