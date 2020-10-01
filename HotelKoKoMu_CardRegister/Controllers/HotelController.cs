using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelKoKoMu_CardRegister.Models;
using HotelKoKoMu_CardRegister.ContextDB;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using System.IO;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using ActionNameAttribute = System.Web.Mvc.ActionNameAttribute;
using System.Web.Http.Results;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hotel_System()
        {
            return View();
        }

        public ActionResult Guest_InformationList()
        {
            return View();
        }

        public ActionResult Guest_InformationNew()
        {
            return View();
        }

        [HttpPost]
        [ActionName("GuestInfo_Save")]
        public IHttpActionResult GuestInfo_Save(CardRegisterInfo cardRegisterInfo)
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
            cardRegisterInfo.Sqlprms[18] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate.ToString("yyyyMMdd"));
            cardRegisterInfo.Sqlprms[19] = new NpgsqlParameter("@createddate", currentDate);
            string sql = "insert into trn_guestinformation(created_date,systemid, pmsid, pmspassword, hotel_code, machineno, systemdate, reservationno, roomno, arrivaldate_hotel, departuredate_hotel, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,flag) " +
               @"values(@createddate,@SystemID, @PmsID, @PmsPassword, @hotelcode, @MachineNo, @systemdate, @reservationno, @roomno, @arrDate, @deptDate, @guestName,@kanaName, @zipcode, @tel, @address1, @address2, @company, @nationality, @PmsPassword,'0')";
            #endregion

            return Ok(bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms));

        }



    }
}