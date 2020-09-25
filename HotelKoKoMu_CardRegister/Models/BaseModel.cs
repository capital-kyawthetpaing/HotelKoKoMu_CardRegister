using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace HotelKoKoMu_CardRegister.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string SPName { get; set; }
        public NpgsqlParameter[] Sqlprms { get; set; }       
    }
}