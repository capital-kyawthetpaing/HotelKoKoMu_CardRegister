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
using Newtonsoft.Json;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelAPIController : ApiController
    {
        /// <summary>
        /// check login user is exist or not
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("checkLogin")]
        public async Task<IHttpActionResult> checkLogin(LoginInfo loginInfo)
        {
            BaseDL bdl = new BaseDL();
            var loginStatus = new object();
            NpgsqlParameter[] para = new NpgsqlParameter[3];
            para[0] = new NpgsqlParameter("@hotelcode", loginInfo.HotelCode);
            para[1] = new NpgsqlParameter("@usercode", loginInfo.UserCode);
            para[2] = new NpgsqlParameter("@password", loginInfo.Password);            
            string sql1 = "select hotel_code,usercode,username from mst_hoteluser where hotel_code = @hotelcode and usercode=@usercode and password=@password and status='1'";
            DataTable dt =await bdl.SelectDataTable(sql1, para);
            if (dt.Rows.Count == 0)
            {
                NpgsqlParameter[] para1 = new NpgsqlParameter[1];
                para1[0] = new NpgsqlParameter("@hotelcode", loginInfo.HotelCode);
                string sql2 = "select hotel_code from mst_hotel where hotel_code = @hotelcode";
                DataTable dthotelcode = await bdl.SelectDataTable(sql2, para1);
                if (dthotelcode.Rows.Count == 0)
                    loginStatus = new { Result = 0 }; //invalid hotel code
                else
                {
                    NpgsqlParameter[] para2 = new NpgsqlParameter[1];
                    para2[0] = new NpgsqlParameter("@usercode", loginInfo.UserCode);
                    string sql3 = "select usercode from mst_hoteluser where usercode=@usercode";
                    DataTable dtusercode = await bdl.SelectDataTable(sql3, para2);
                    if(dtusercode.Rows.Count==0)
                        loginStatus = new { Result = 1 }; // invalid user code
                    else
                        loginStatus = new { Result = 2 }; // invalid  password               
                }                        
            }
            else
                loginStatus = new { Result = dt };
            return Ok(loginStatus);
        }


        [HttpPost]
        [ActionName("Search_GuestData")]
        public async Task<IHttpActionResult> Search_GuestData(CardRegisterInfo cardInfo)
        {
            BaseDL bdl = new BaseDL();
            cardInfo.Sqlprms = new NpgsqlParameter[1];
            cardInfo.Sqlprms[0] = new NpgsqlParameter("@arrivaldate", cardInfo.ArrivalDate);

            string sql_cmd = "select arrival_date,departure_date,roomno,guestname_text,kananame_text,concat(address1_text,address2_text) as address from trn_guestinformation_test where arrival_date >= @arrivaldate";
            DataTable dt = await bdl.SelectDataTable(sql_cmd, cardInfo.Sqlprms);
            return Ok(dt);
        }

        /// <summary>
        /// get hotel information based on hotel no
        /// </summary>
        /// <param name="hotelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("getHotelInformation")]
        public async Task<IHttpActionResult> getHotelInformation(HotelInfo hotelInfo)
        {
            BaseDL bdl = new BaseDL();
            hotelInfo.Sqlprms = new NpgsqlParameter[1];
            hotelInfo.Sqlprms[0] = new NpgsqlParameter("@hotel_code", hotelInfo.HotelNo);
            string cmdText = "Select hotel_name,logo_data from mst_hotel where hotel_code=@hotel_code";
            DataTable dt = await bdl.SelectDataTable(cmdText, hotelInfo.Sqlprms);
            return Ok(dt);
        }
    }
}
