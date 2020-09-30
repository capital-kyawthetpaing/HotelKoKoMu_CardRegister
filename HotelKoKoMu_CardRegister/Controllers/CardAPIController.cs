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
using System.Net.Http.Formatting;
using System.Web.Http.ModelBinding;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.IO;
using System.Drawing;

namespace HotelKoKoMu_CardRegister.Controllers
{

    public class CardAPIController : ApiController
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string culture = HttpContext.Current.Request.Cookies["culture"].Value;

        #region 
        //[HttpPost]
        //[ActionName("SaveCardInformation")]
        //public IHttpActionResult SaveCardInformation(CardRegisterModel model)
        //{
        //    string cmdText = "";
        //    BaseDL bdl = new BaseDL();
        //    model.Sqlprms = new NpgsqlParameter[16];
        //    if (model.Mode == "1")
        //    {
        //        #region for keyboard input

        //        model.Sqlprms[0] = new NpgsqlParameter("@guestName", model.GuestName);
        //        if (culture == "Ja")
        //            model.Sqlprms[1] = new NpgsqlParameter("@kanaName", model.KanaName);
        //        else
        //            model.Sqlprms[1] = new NpgsqlParameter("@kanaName", model.GuestName);
        //        model.Sqlprms[2] = new NpgsqlParameter("@postalCode", model.PostalCode);
        //        model.Sqlprms[3] = new NpgsqlParameter("@phoneNo", model.PhoneNo);
        //        model.Sqlprms[4] = new NpgsqlParameter("@address1", model.Address1);
        //        model.Sqlprms[5] = new NpgsqlParameter("@address2", model.Address2);
        //        model.Sqlprms[6] = new NpgsqlParameter("@workplace", model.WorkPlace);
        //        model.Sqlprms[7] = new NpgsqlParameter("@nationality", model.Nationality);
        //        model.Sqlprms[8] = new NpgsqlParameter("@passport", model.Passport);
        //        model.Sqlprms[9] = new NpgsqlParameter("@sign", ConvertBase64StringToByte(model.Sign));

        //        cmdText = "insert into trn_guestinformation(guestname_text,kananame_text,postalcode_text," +
        //                  @"phoneno_text,address1_text,address2_text,workplace_text,nationality_text," +
        //                  @"passportno_text,arrival_date,departure_date,sign,creator,updator,created_date,updated_date)" +
        //                  @"values(@guestName,@kanaName,@postalCode,@phoneNo,@address1,@address2,@workplace,@nationality,@passport,@arrDate,@deptDate,@sign,@creator,@updator,@createddate,@updateddate)";
        //        #endregion
        //    }
        //    else
        //    {
        //        #region for hand writing

        //        model.Sqlprms[0] = new NpgsqlParameter("@guestNameimg", ConvertBase64StringToByte(model.GuestName));
        //        if (culture == "Ja")
        //            model.Sqlprms[1] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.KanaName));
        //        else
        //            model.Sqlprms[1] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.GuestName));
        //        model.Sqlprms[2] = new NpgsqlParameter("@postalCodeimg", ConvertBase64StringToByte(model.PostalCode));
        //        model.Sqlprms[3] = new NpgsqlParameter("@phoneNoimg", ConvertBase64StringToByte(model.PhoneNo));
        //        model.Sqlprms[4] = new NpgsqlParameter("@addressimg1", ConvertBase64StringToByte(model.Address1));
        //        model.Sqlprms[5] = new NpgsqlParameter("@addressimg2", ConvertBase64StringToByte(model.Address2));
        //        model.Sqlprms[6] = new NpgsqlParameter("@workPlaceimg", ConvertBase64StringToByte(model.WorkPlace));
        //        model.Sqlprms[7] = new NpgsqlParameter("@nationalityimg", ConvertBase64StringToByte(model.Nationality));
        //        model.Sqlprms[8] = new NpgsqlParameter("@passportimg", ConvertBase64StringToByte(model.Passport));
        //        model.Sqlprms[9] = new NpgsqlParameter("@signimg", ConvertBase64StringToByte(model.Sign));

        //        cmdText = "insert into trn_guestinformation(guestname_handwriting,kananame_handwriting,postalcode_handwriting," +
        //                 @"phoneno_handwriting,address1_handwriting,address2_handwriting,workplace_handwriting,nationality_handwriting," +
        //                 @"passportno_handwriting,arrival_date,departure_date,sign,creator,updator,created_date,updated_date)" +
        //                 @"values(@guestNameimg,@kanaNameimg,@postalCodeimg,@phoneNoimg,@addressimg1,@addressimg2,@workplaceimg,@nationalityimg,@passportimg,@arrDate,@deptDate,@signimg,@creator,@updator,@createddate,@updateddate)";
        //        #endregion
        //    }
        //    DateTime currentDate = DateTime.Now;
        //    model.Sqlprms[10] = new NpgsqlParameter("@arrDate", model.ArrivalDate);
        //    model.Sqlprms[11] = new NpgsqlParameter("@deptDate", model.DepartureDate);
        //    model.Sqlprms[12] = new NpgsqlParameter("@creator", model.CreatedBy);
        //    model.Sqlprms[13] = new NpgsqlParameter("@updator", model.UpdatedBy);
        //    model.Sqlprms[14] = new NpgsqlParameter("@createddate", currentDate);
        //    model.Sqlprms[15] = new NpgsqlParameter("@updateddate", currentDate);
        //    return Ok(bdl.InsertUpdateDeleteData(cmdText, model.Sqlprms));
        //}


        //[HttpPost]
        //[ActionName("SaveCardInformation")]
        //public IHttpActionResult SaveCardInformation(CardRegisterModel model)
        //{
        //    #region
        //    string cmdText = "CALL saveguestinfo(@guestname,@guestnamehw,@kananame,@kananamehw,@postalcode,@postalcodehw,@phoneno,@phonenohw,@address1,@addresshw1,@address2,@addresshw2,@workplace,@workplacehw,@nationality,@nationalityhw,@passportno,@passportnohw,@sign,@arrivaldate,@departuredate,@creator,@updator)";            
        //    BaseDL bdl = new BaseDL();
        //    model.Sqlprms = new NpgsqlParameter[23];

        //    model.Sqlprms[0] = model.Mode == "1" ? new NpgsqlParameter("@guestname", model.GuestName) : new NpgsqlParameter("@guestname", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[1] = model.Mode == "2" ? new NpgsqlParameter("@guestnamehw", ConvertBase64StringToByte(model.GuestName)) : new NpgsqlParameter("@guestnamehw", NpgsqlDbType.Bytea) { Value=""};
        //    if (culture == "Ja")
        //        model.Sqlprms[2] = model.Mode == "1" ? new NpgsqlParameter("@kananame", model.KanaName) : new NpgsqlParameter("@kananame", NpgsqlDbType.Varchar) { Value = "" };
        //    else
        //        model.Sqlprms[2] = model.Mode == "1" ? new NpgsqlParameter("@kananame", model.GuestName) : new NpgsqlParameter("@kananame", NpgsqlDbType.Varchar) { Value = "" };

        //    if (culture == "Ja")
        //        model.Sqlprms[3] = model.Mode == "2" ? new NpgsqlParameter("@kananamehw", ConvertBase64StringToByte(model.KanaName)) : new NpgsqlParameter("@kananamehw", NpgsqlDbType.Bytea) { Value = "" };
        //    else
        //        model.Sqlprms[3] = model.Mode == "2" ? new NpgsqlParameter("@kananamehw", ConvertBase64StringToByte(model.GuestName)) : new NpgsqlParameter("@kananamehw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[4] = model.Mode == "1" ? new NpgsqlParameter("@postalcode", model.PostalCode) : new NpgsqlParameter("@postalcode", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[5] = model.Mode == "2" ? new NpgsqlParameter("@postalcodehw", ConvertBase64StringToByte(model.PostalCode)) : new NpgsqlParameter("@postalcodehw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[6] = model.Mode == "1" ? new NpgsqlParameter("@phoneno", model.PhoneNo) : new NpgsqlParameter("@phoneno", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[7] = model.Mode == "2" ? new NpgsqlParameter("@phonenohw", ConvertBase64StringToByte(model.PhoneNo)) : new NpgsqlParameter("@phonenohw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[8] = model.Mode == "1" ? new NpgsqlParameter("@address1", model.Address1) : new NpgsqlParameter("@address1", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[9] = model.Mode == "2" ? new NpgsqlParameter("@addresshw1", ConvertBase64StringToByte(model.Address1)) : new NpgsqlParameter("@addresshw1", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[10] = model.Mode == "1" ? new NpgsqlParameter("@address2", model.Address2) : new NpgsqlParameter("@address2", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[11] = model.Mode == "2" ? new NpgsqlParameter("@addresshw2", ConvertBase64StringToByte(model.Address2)) : new NpgsqlParameter("@addresshw2", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[12] = model.Mode == "1" ? new NpgsqlParameter("@workplace", model.WorkPlace) : new NpgsqlParameter("@workplace", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[13] = model.Mode == "2" ? new NpgsqlParameter("@workplacehw", ConvertBase64StringToByte(model.WorkPlace)) : new NpgsqlParameter("@workplacehw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[14] = model.Mode == "1" ? new NpgsqlParameter("@nationality", model.Nationality) : new NpgsqlParameter("@nationality", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[15] = model.Mode == "2" ? new NpgsqlParameter("@nationalityhw", ConvertBase64StringToByte(model.Nationality)) : new NpgsqlParameter("@nationalityhw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[16] = model.Mode == "1" ? new NpgsqlParameter("@passportno", model.Passport) : new NpgsqlParameter("@passportno", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[17] = model.Mode == "2" ? new NpgsqlParameter("@passportnohw", ConvertBase64StringToByte(model.Passport)) : new NpgsqlParameter("@passportnohw", NpgsqlDbType.Bytea) { Value = "" };

        //    model.Sqlprms[18] = new NpgsqlParameter("@sign", ConvertBase64StringToByte(model.Sign));
        //    model.Sqlprms[19] = new NpgsqlParameter("@arrivaldate", NpgsqlDbType.Date) { Value=model.ArrivalDate.Date};
        //    model.Sqlprms[20] = new NpgsqlParameter("@departuredate", NpgsqlDbType.Date) { Value = model.DepartureDate.Date };
        //    model.Sqlprms[21] = new NpgsqlParameter("@creator", NpgsqlDbType.Varchar) { Value = "" };
        //    model.Sqlprms[22] = new NpgsqlParameter("@updator", NpgsqlDbType.Varchar) { Value = "" };
        //    return Ok(bdl.InsertUpdateDeleteData(cmdText, model.Sqlprms));
        //    #endregion
        //}


        //to convert base64string to byte 
        #endregion

        #region 
        /// <summary>
        /// update guest information 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveGuestInformation")]
        public IHttpActionResult SaveGuestInformation(CardRegisterModel model)
        {
            BaseDL bdl = new BaseDL();
            DateTime currentDate = DateTime.Now;
            model.Sqlprms = new NpgsqlParameter[19];
            #region update guest information
            model.Sqlprms[0] = new NpgsqlParameter("@guestName", model.GuestName);
            if (culture == "Ja")
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
            model.Sqlprms[9] = new NpgsqlParameter("@arrDate", model.ArrivalDate);
            model.Sqlprms[10] = new NpgsqlParameter("@deptDate", model.DepartureDate);
            model.Sqlprms[11] = new NpgsqlParameter("@updator", model.UpdatedBy);
            model.Sqlprms[12] = new NpgsqlParameter("@createddate", model.CreatedDate);
            model.Sqlprms[13] = new NpgsqlParameter("@updateddate", currentDate);

            string fileName = model.SystemDate.ToString("yyyyMMdd") + model.ReservationNo + model.RoomNo + DateTime.Now.ToString("yyyyMMdd") + model.HotelCode + ".jpg";
            SaveImage(model.Sign, model.HotelCode, fileName);
            model.Sqlprms[14] = new NpgsqlParameter("@filename", fileName);
           
            model.Sqlprms[15] = new NpgsqlParameter("@hotelcode", model.HotelCode);
            model.Sqlprms[16] = new NpgsqlParameter("@reservationno", model.ReservationNo);
            model.Sqlprms[17] = new NpgsqlParameter("@roomno", model.RoomNo);
            model.Sqlprms[18] = new NpgsqlParameter("@systemdate", model.SystemDate);

            string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,postalcode_text=@postalCode,phoneno_text=@phoneNo,";
            sql += "address1_text=@address1,address2_text=@address2,workplace_text=@workplace,nationality_text=@nationality,passportno_text=@passport,";
            sql += "arrival_date=@arrDate,departure_date=@deptDate,updator=@updator,updated_date=@updateddate,sign_filename=@filename where hotel_code=@hotelcode and";
            sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@systemdate AS DATE)";

            #endregion

            return Ok(bdl.InsertUpdateDeleteData(sql, model.Sqlprms));
        }
        #endregion

        /// <summary>
        /// Convert base64String to byte array based on incoming hand writing
        /// </summary>
        /// <param name="common"></param>
        /// <returns></returns>
        /// 
        public byte[] ConvertBase64StringToByte(string common)
        {
            byte[] byteCommon = null;
            if (!string.IsNullOrEmpty(common))
            {
                string[] arrCommon = common.Split(',');
                byteCommon = Convert.FromBase64String(arrCommon[1]);
            }
            return byteCommon;
        }

        public void SaveImage(string common,string HotelCode,string fileName)
        {
           var dirPath = HttpContext.Current.Server.MapPath("~/"+HotelCode);           
            byte[] bytes = null;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!string.IsNullOrEmpty(common))
            {
                string[] arrCommon = common.Split(',');
                bytes = Convert.FromBase64String(arrCommon[1]);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
                dirPath = dirPath + "//" + fileName;
                image.Save(dirPath);

            }
        }

        /// <summary>
        /// get hotel information based on hotel no
        /// </summary>
        /// <param name="hotelModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHotelInformation")]
        public IHttpActionResult GetHotelInformation(HotelModel hotelModel)
        {
            BaseDL bdl = new BaseDL();
            hotelModel.Sqlprms = new NpgsqlParameter[1];
            hotelModel.Sqlprms[0] = new NpgsqlParameter("@hotelno", hotelModel.HotelNo);
            string cmdText = "Select hotel_name,logo_data from mst_hotel where hotel_no=@hotelno";
            DataTable dt = bdl.SelectDataTable(cmdText, hotelModel.Sqlprms);
            return Ok(dt);
        }
    }
}
