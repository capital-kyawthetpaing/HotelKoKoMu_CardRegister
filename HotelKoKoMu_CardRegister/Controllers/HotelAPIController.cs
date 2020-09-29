using HotelKoKoMu_CardRegister.ContextDB;
using HotelKoKoMu_CardRegister.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelAPIController : ApiController
    {

        [HttpPost]
        [ActionName("GetGuestInformationFromHotel")]
        public IHttpActionResult GetGuestInformationFromHotel(CardRegisterModel hotelModel)
        {
            BaseDL bdl = new BaseDL();
            hotelModel.Sqlprms = new NpgsqlParameter[5];
            hotelModel.Sqlprms[0] = new NpgsqlParameter("@systemid", "e-card");
            hotelModel.Sqlprms[1] = new NpgsqlParameter("@pmsid", "FMFS");
            hotelModel.Sqlprms[2] = new NpgsqlParameter("@pmspassword", "Abc123#");
            hotelModel.Sqlprms[3] = new NpgsqlParameter("@hotelcode", "000001");
            hotelModel.Sqlprms[4] = new NpgsqlParameter("@machineno", "01");
            string cmdText = "Select * from hotel_guestinformation where systemid=@systemid and pmsid=@pmsid and pmspassword=@pmspassword and machineno=@machineno";
            DataTable dt = bdl.SelectDataTable(cmdText, hotelModel.Sqlprms);
            return Ok(dt);
        }

        [HttpPost]
        [ActionName("GuestInformation")]
        public IHttpActionResult GuestInformation(HotelSystemModel hotelModel)
        {
            BaseDL bdl = new BaseDL();
            hotelModel.Sqlprms = new NpgsqlParameter[0];
            string cmdText = "Select * from hotel_guestinformation";
            DataTable dt = bdl.SelectDataTable(cmdText, hotelModel.Sqlprms);
            return Ok(dt);
        }

    }
}
