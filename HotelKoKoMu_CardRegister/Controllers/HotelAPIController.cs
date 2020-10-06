using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotelKoKoMu_CardRegister.Models;
using HotelKoKoMu_CardRegister.ContextDB;
using Npgsql;
using System.Data;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelAPIController : ApiController
    {

        [HttpPost]
        [ActionName("ValidateLogin")]
        public IHttpActionResult ValidateLogin(LoginInfo info)
        {
            BaseDL bdl = new BaseDL();
            info.Sqlprms = new NpgsqlParameter[3];
            info.Sqlprms[0] = new NpgsqlParameter("@hotelcode", info.HotelCode);
            info.Sqlprms[1] = new NpgsqlParameter("@userid", info.UserID);
            info.Sqlprms[2] = new NpgsqlParameter("@password", info.Password);
            
            string sql_cmd = "select * from mst_hoteluser where hotel_code = @hotelcode and usercode=@userid and password=@password";
            DataTable dt = bdl.SelectDataTable(sql_cmd, info.Sqlprms);
            return Ok(dt);
        }
    }
}
