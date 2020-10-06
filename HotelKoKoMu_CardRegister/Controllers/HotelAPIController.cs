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
using System.Threading.Tasks;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelAPIController : ApiController
    {

        [HttpPost]
        [ActionName("ValidateLogin")]
        public async Task<IHttpActionResult> ValidateLogin(LoginInfo info)
        {
            BaseDL bdl = new BaseDL();
            info.Sqlprms = new NpgsqlParameter[3];
            info.Sqlprms[0] = new NpgsqlParameter("@hotelcode", info.HotelCode);
            info.Sqlprms[1] = new NpgsqlParameter("@userid", info.UserID);
            info.Sqlprms[2] = new NpgsqlParameter("@password", info.Password);
            
            string sql_cmd = "select * from mst_hoteluser where hotel_code = @hotelcode and usercode=@userid and password=@password";
            DataTable dt =await bdl.SelectDataTable(sql_cmd, info.Sqlprms);
            return Ok(dt);
        }

        [HttpPost]
        [ActionName("Search_GuestData")]
        public async Task<IHttpActionResult> Search_GuestData(CardRegisterInfo cardInfo)
        {
            BaseDL bdl = new BaseDL();
            cardInfo.Sqlprms = new NpgsqlParameter[1];
            cardInfo.Sqlprms[0] = new NpgsqlParameter("@arrivaldate", cardInfo.ArrivalDate);

            string sql_cmd = "select * from trn_guestinformation_test where arrival_date >= @arrivaldate";
            DataTable dt =await bdl.SelectDataTable(sql_cmd, cardInfo.Sqlprms);
            return Ok(dt);
        }
    }
}
