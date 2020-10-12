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
            var Status = new object();
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[0];
            string condition = string.Empty;
            condition = " and h.hotel_code='" + searchGuestInfo.HotelCode + "'";
            if (!(searchGuestInfo.ArrivalFromDate == DateTime.MinValue && searchGuestInfo.ArrivalToDate == DateTime.MinValue))
                condition += " and Cast(arrival_date as Date) between Cast('" + searchGuestInfo.ArrivalFromDate + "' as Date) and Cast('" + searchGuestInfo.ArrivalToDate + "' as Date)";
            if (!string.IsNullOrEmpty(searchGuestInfo.RoomNo))
                condition += " and roomno='" + searchGuestInfo.RoomNo + "'";
            if (!string.IsNullOrEmpty(searchGuestInfo.GuestName))
                condition += " and (guestname_hotel like '%" + searchGuestInfo.GuestName + "%' or kananame_hotel like '%" + searchGuestInfo.GuestName + "%')";
            string sql_cmd = "select arrival_date,departure_date,Case when roomno_fill_text isnull then roomno else lpad(roomno,hotel_roomno_count,roomno_fill_text) end as roomno,";
            sql_cmd += " guestname_text,kananame_text,trim(concat_ws(',', address1_text, address2_text),',') as address,h.hotel_code,imagedata from trn_guestinformation guest inner join mst_hotel h";
            sql_cmd += " on guest.hotel_code=h.hotel_code where complete_flag=1" + condition + " order by arrival_date,roomno,kananame_text";
            DataTable dt = await bdl.SelectDataTable(sql_cmd, Sqlprms);
            Status = new { Result = dt };
            return Ok(Status);
        }

        //[HttpPost]
        //[ActionName("searchGuestData")]
        //public async Task<IHttpActionResult> searchGuestData(SearchGuestInfo searchGuestInfo)
        //{
        //    var Status = new object();
        //    int roomno_count = 0;
        //    string filltext = string.Empty;
        //    DataTable dtinfo = GetRoomNo_Info(searchGuestInfo.HotelCode);
        //    if (dtinfo.Rows.Count > 0)
        //    {
        //        roomno_count = Convert.ToInt32(dtinfo.Rows[0]["hotel_roomno_count"].ToString());
        //        filltext = dtinfo.Rows[0]["roomno_fill_text"].ToString();

        //        if (roomno_count >= searchGuestInfo.RoomNo.Length)
        //        {
        //            BaseDL bdl = new BaseDL();
        //            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[0];
        //            string condition = string.Empty;
        //            condition = " and hotel_code='" + searchGuestInfo.HotelCode + "'";
        //            if (!(searchGuestInfo.ArrivalFromDate == DateTime.MinValue && searchGuestInfo.ArrivalToDate == DateTime.MinValue))
        //                condition += " and Cast(arrival_date as Date) between Cast('" + searchGuestInfo.ArrivalFromDate + "' as Date) and Cast('" + searchGuestInfo.ArrivalToDate + "' as Date)";
        //            if (!string.IsNullOrEmpty(searchGuestInfo.RoomNo))
        //                condition += " and lpad(roomno, " + roomno_count + ",'" + filltext + "')=lpad('" + searchGuestInfo.RoomNo + "'," + roomno_count + ",'" + filltext + "')";
        //            if (!string.IsNullOrEmpty(searchGuestInfo.GuestName))
        //                condition += " and (guestname_hotel like '%" + searchGuestInfo.GuestName + "%' or kananame_hotel like '%" + searchGuestInfo.GuestName + "%')";
        //            string sql_cmd = "select arrival_date,departure_date,roomno,guestname_text,kananame_text,concat_ws(',', address1_text, address2_text) as address,hotel_code,imagedata from trn_guestinformation where complete_flag=1" + condition + " order by arrival_date,roomno,kananame_text";
        //            DataTable dt = await bdl.SelectDataTable(sql_cmd, Sqlprms);
        //            if (dt.Rows.Count == 0)
        //                Status = new { Result = 1 };//no data search
        //            else
        //                Status = new { Result = dt };
        //        }
        //    }
        //    else
        //    {
        //        Status = new { Result = 0 };//roomno count is less.
        //    }
        //    return Ok(Status);
        //}

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
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[1];
            Sqlprms[0] = new NpgsqlParameter("@hotel_code", hotelInfo.HotelNo);
            string cmdText = "Select hotel_name,logo_data from mst_hotel where hotel_code=@hotel_code";
            DataTable dt = await bdl.SelectDataTable(cmdText,Sqlprms);
            return Ok(dt);
        }


        [HttpPost]
        [ActionName("CheckRoomNo")]
        public async Task<IHttpActionResult> CheckRoomNo(HotelInfo hotelInfo)
        {
            BaseDL bdl = new BaseDL();
            int roomno_count = 0;
            string filltext = string.Empty;
            string result = hotelInfo.RoomNo;
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[1];
            Sqlprms[0] = new NpgsqlParameter("@hotel_code", hotelInfo.HotelNo);
            string cmdText = "Select hotel_code,hotel_roomno_count,roomno_fill_text from mst_hotel where hotel_code=@hotel_code";
            DataTable dt = await bdl.SelectDataTable(cmdText, Sqlprms);
            if(dt.Rows.Count> 0)
            {
                if (!String.IsNullOrWhiteSpace(dt.Rows[0]["hotel_roomno_count"].ToString()) && dt.Rows[0]["hotel_roomno_count"].ToString() != "")
                {
                    roomno_count  = Convert.ToInt32(dt.Rows[0]["hotel_roomno_count"].ToString());
                    if (roomno_count >= result.Length)
                    {
                        if (!String.IsNullOrWhiteSpace(dt.Rows[0]["roomno_fill_text"].ToString()))
                        {
                            filltext = dt.Rows[0]["roomno_fill_text"].ToString();
                            result = result.PadLeft(Convert.ToInt32(roomno_count), Convert.ToChar(filltext));
                        }
                        else if (dt.Rows[0]["roomno_fill_text"].ToString() == " "){
                            result = result.PadLeft(Convert.ToInt32(roomno_count), ' ');
                        }
                        else
                        {
                            result = hotelInfo.RoomNo;
                        }
                    }
                    else
                    {
                        result = "Error";
                    }
                } 
            }
            else
            {
                result = hotelInfo.RoomNo;
            }

            return Ok(result);
        }

      
        public DataTable GetRoomNo_Info(string hotelcode)
        {
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[1];
            Sqlprms[0] = new NpgsqlParameter("@hotel_code", hotelcode);
            string cmdText = "Select hotel_roomno_count,roomno_fill_text from mst_hotel where hotel_code=@hotel_code";
            DataTable dt =  bdl.SelectDataTable_Info(cmdText, Sqlprms);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            return dt;
        }
    }
}
