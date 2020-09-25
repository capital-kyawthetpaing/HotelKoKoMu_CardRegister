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
using System.Configuration;
using NpgsqlTypes;
using System.Web;

namespace HotelKoKoMu_CardRegister.Controllers
{
   
    public class CardAPIController : ApiController
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string culture = HttpContext.Current.Request.Cookies["culture"].Value;
        [HttpPost]
        [ActionName("SaveCardInformation")]
        public IHttpActionResult SaveCardInformation(CardRegisterModel model)
        {
            string cmdText = "";
            BaseDL bdl = new BaseDL();
            model.Sqlprms = new NpgsqlParameter[16];
            if (model.Mode == "1")
            {
                #region for keyboard input
               
                model.Sqlprms[0] = new NpgsqlParameter("@guestName", model.GuestName);
                if(culture=="Ja")
                    model.Sqlprms[1] = new NpgsqlParameter("@kanaName", model.KanaName);
                else
                    model.Sqlprms[1] = new NpgsqlParameter("@kanaName", model.GuestName);
                model.Sqlprms[2] = new NpgsqlParameter("@postalCode", model.PostalCode);
                model.Sqlprms[3] = new NpgsqlParameter("@phoneNo", model.PhoneNo);
                model.Sqlprms[4] = new NpgsqlParameter("@address1", model.Address1);
                model.Sqlprms[5] = new NpgsqlParameter("@address2", model.Address2);
                model.Sqlprms[6] = new NpgsqlParameter("@workplace", model.WorkPlace);
                model.Sqlprms[7] = new NpgsqlParameter("@nationality", model.Nationality);
                model.Sqlprms[8] = new NpgsqlParameter("@passport", model.Passport);
                model.Sqlprms[9] = new NpgsqlParameter("@sign", ConvertBase64StringToByte(model.Sign));

                cmdText = "insert into mst_cardinformation(guestname_text,kananame_text,postalcode_text," +
                          @"phoneno_text,address1_text,address2_text,workplace_text,nationality_text," +
                          @"passportno_text,arrival_date,departure_date,sign,creator,updator,created_date,updated_date)" +
                          @"values(@guestName,@kanaName,@postalCode,@phoneNo,@address1,@address2,@workplace,@nationality,@passport,@arrDate,@deptDate,@sign,@creator,@updator,@createddate,@updateddate)";
                #endregion
            }
            else
            {
                #region for hand writing

                model.Sqlprms[0] = new NpgsqlParameter("@guestNameimg", ConvertBase64StringToByte(model.GuestName));
                if(culture=="Ja")
                    model.Sqlprms[1] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.KanaName));
                else
                    model.Sqlprms[1] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.GuestName));
                model.Sqlprms[2] = new NpgsqlParameter("@postalCodeimg", ConvertBase64StringToByte(model.PostalCode));
                model.Sqlprms[3] = new NpgsqlParameter("@phoneNoimg", ConvertBase64StringToByte(model.PhoneNo));
                model.Sqlprms[4] = new NpgsqlParameter("@addressimg1", ConvertBase64StringToByte(model.Address1));
                model.Sqlprms[5] = new NpgsqlParameter("@addressimg2", ConvertBase64StringToByte(model.Address2));
                model.Sqlprms[6] = new NpgsqlParameter("@workPlaceimg", ConvertBase64StringToByte(model.WorkPlace));
                model.Sqlprms[7] = new NpgsqlParameter("@nationalityimg", ConvertBase64StringToByte(model.Nationality));
                model.Sqlprms[8] = new NpgsqlParameter("@passportimg", ConvertBase64StringToByte(model.Passport));
                model.Sqlprms[9] = new NpgsqlParameter("@signimg", ConvertBase64StringToByte(model.Sign));

                cmdText = "insert into mst_cardinformation(guestname_handwriting,kananame_handwriting,postalcode_handwriting," +
                         @"phoneno_handwriting,address1_handwriting,address2_handwriting,workplace_handwriting,nationality_handwriting," +
                         @"passportno_handwriting,arrival_date,departure_date,sign,creator,updator,created_date,updated_date)" +
                         @"values(@guestNameimg,@kanaNameimg,@postalCodeimg,@phoneNoimg,@addressimg1,@addressimg2,@workplaceimg,@nationalityimg,@passportimg,@arrDate,@deptDate,@signimg,@creator,@updator,@createddate,@updateddate)";
                #endregion
            }
            DateTime currentDate = DateTime.Now;
            model.Sqlprms[10] = new NpgsqlParameter("@arrDate", model.ArrivalDate);
            model.Sqlprms[11] = new NpgsqlParameter("@deptDate", model.DepartureDate);
            model.Sqlprms[12] = new NpgsqlParameter("@creator", model.CreatedBy);
            model.Sqlprms[13] = new NpgsqlParameter("@updator", model.UpdatedBy);
            model.Sqlprms[14] = new NpgsqlParameter("@createddate", currentDate);
            model.Sqlprms[15] = new NpgsqlParameter("@updateddate", currentDate);
            return Ok(bdl.InsertUpdateDeleteData(cmdText, model.Sqlprms));
        }

        //to convert base64string to byte 
        public byte[] ConvertBase64StringToByte(string common)
        {
            byte[] byteCommon=null;
            if (!string.IsNullOrEmpty(common))
            {
                string[] arrCommon = common.Split(',');
                byteCommon = Convert.FromBase64String(arrCommon[1]);
            }
            return byteCommon;
        }


        [HttpPost]
        [ActionName("GetHotelInformation")]
        public IHttpActionResult GetHotelInformation(HotelModel hotelModel)
        {
            BaseDL bdl = new BaseDL();
            hotelModel.Sqlprms = new NpgsqlParameter[1];
            hotelModel.Sqlprms[0] = new NpgsqlParameter("@hotelno", hotelModel.HotelNo);
            string cmdText = "Select * from mst_hotel where hotel_no=@hotelno";
            DataTable dt = bdl.SelectDataTable(cmdText, hotelModel.Sqlprms);
            return Ok(dt);
        }
    }
}
