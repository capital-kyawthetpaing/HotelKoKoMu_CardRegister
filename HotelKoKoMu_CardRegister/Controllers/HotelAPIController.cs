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

        [HttpPost]
        [ActionName("GuestInfo_Save")]
        public async Task<IHttpActionResult> GuestInfo_Save(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            DateTime currentDate = DateTime.Now;
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[20];
            #region insert guest information
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@guestName", cardRegisterInfo.GuestName);
            cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.KanaName);
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@zipcode", cardRegisterInfo.ZipCode);
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@tel", cardRegisterInfo.Tel);
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@address1", cardRegisterInfo.Address1);
            cardRegisterInfo.Sqlprms[5] = new NpgsqlParameter("@address2", cardRegisterInfo.Address2);
            cardRegisterInfo.Sqlprms[6] = new NpgsqlParameter("@company", cardRegisterInfo.Company);
            cardRegisterInfo.Sqlprms[7] = new NpgsqlParameter("@nationality", cardRegisterInfo.Nationality);
            cardRegisterInfo.Sqlprms[8] = new NpgsqlParameter("@passport", cardRegisterInfo.Passport);
            cardRegisterInfo.Sqlprms[9] = new NpgsqlParameter("@arrDate", cardRegisterInfo.ArrivalDate);
            cardRegisterInfo.Sqlprms[10] = new NpgsqlParameter("@deptDate", cardRegisterInfo.DepartureDate);
            cardRegisterInfo.Sqlprms[11] = new NpgsqlParameter("@SystemID", cardRegisterInfo.SystemID);
            cardRegisterInfo.Sqlprms[12] = new NpgsqlParameter("@PmsID", cardRegisterInfo.PmsID);
            cardRegisterInfo.Sqlprms[13] = new NpgsqlParameter("@PmsPassword", cardRegisterInfo.PmsPassword);
            cardRegisterInfo.Sqlprms[14] = new NpgsqlParameter("@MachineNo", cardRegisterInfo.MachineNo);
            cardRegisterInfo.Sqlprms[15] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            cardRegisterInfo.Sqlprms[16] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            cardRegisterInfo.Sqlprms[17] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
            cardRegisterInfo.Sqlprms[18] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);
            cardRegisterInfo.Sqlprms[19] = new NpgsqlParameter("@createddate", currentDate);

            string sql = "insert into trn_guestinformation(created_date,systemid, pmsid, pmspassword, hotel_code, machineno, systemdate, reservationno, roomno, arrivaldate_hotel, departuredate_hotel, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,flag,complete_flag) " +
               @"values(@createddate,@SystemID, @PmsID, @PmsPassword, @hotelcode, @MachineNo, @systemdate, @reservationno, @roomno, @arrDate, @deptDate, @guestName,@kanaName, @zipcode, @tel, @address1, @address2, @company, @nationality, @passport,'0','0')";
            #endregion

            return Ok(await bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms));
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
