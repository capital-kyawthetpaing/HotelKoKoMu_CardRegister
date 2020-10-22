using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_RegistrationCardSystem.Models
{
    public class ReturnMessageInfo
    {
        public string Status { get; set; }
        public string FailureReason { get; set; }
        public string ErrorDescription { get; set; }
    }
}