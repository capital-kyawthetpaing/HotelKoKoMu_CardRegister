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
using NpgsqlTypes;

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
        [ActionName("searchGuestData")]
        public async Task<IHttpActionResult> searchGuestData(SearchGuestInfo searchGuestInfo)
        {
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[0];
            string condition = string.Empty;
            condition =" and hotel_code='"+searchGuestInfo.HotelCode+"'";           
            if(!(searchGuestInfo.ArrivalFromDate==DateTime.MinValue && searchGuestInfo.ArrivalToDate==DateTime.MinValue))
                condition += " and Cast(arrival_date as Date) between Cast('" + searchGuestInfo.ArrivalFromDate + "' as Date) and Cast('" + searchGuestInfo.ArrivalToDate + "' as Date)";
            if (!string.IsNullOrEmpty(searchGuestInfo.RoomNo))
                condition += " and lpad(roomno, 4, '0')=lpad('" + searchGuestInfo.RoomNo + "',4,\'0\')";           
            if (!string.IsNullOrEmpty(searchGuestInfo.GuestName))
                condition += " and (guestname_hotel like '%" +searchGuestInfo.GuestName+"%' or kananame_hotel like '%"+ searchGuestInfo.GuestName + "%')";                
            string sql_cmd = "select arrival_date,departure_date,lpad(roomno, 4, '0') as roomno,guestname_text,kananame_text,concat(address1_text,address2_text) as address,hotel_code,imagedata from trn_guestinformation where flag=1 and complete_flag=1" + condition+ " order by arrival_date,roomno,kananame_text";           
            DataTable dt = await bdl.SelectDataTable(sql_cmd, Sqlprms);
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
