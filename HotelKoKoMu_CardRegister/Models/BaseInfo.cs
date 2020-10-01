using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace HotelKoKoMu_CardRegister.Models
{
    public class BaseInfo
    {
        public string Creator { get; set; }
        public string Updator { get; set; }
        public string SPName { get; set; }
        public NpgsqlParameter[] Sqlprms { get; set; }       
    }
}