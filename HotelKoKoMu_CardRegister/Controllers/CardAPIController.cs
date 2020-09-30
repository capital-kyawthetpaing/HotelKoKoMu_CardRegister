﻿using System;
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
        /// Save and update guest information 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveGuestInformation")]
        public IHttpActionResult SaveGuestInformation(CardRegisterModel model)
        {
            string sql, sql1, result1, result2 = "";
            BaseDL bdl = new BaseDL();
            DateTime currentDate = DateTime.Now;
            model.Sqlprms = new NpgsqlParameter[30];

            #region  save guest information into trn_guestinformation 
            // model.HotelCode = "000002";
            //model.ReservationNo = "00003";
            model.Sqlprms[0] = new NpgsqlParameter("@hotelcode", model.HotelCode);
            model.Sqlprms[1] = new NpgsqlParameter("@reservationno", model.ReservationNo);
            model.Sqlprms[2] = new NpgsqlParameter("@roomno", model.RoomNo);
            model.Sqlprms[3] = new NpgsqlParameter("@systemdate", model.SystemDate);
            model.Sqlprms[4] = new NpgsqlParameter("@guestName", model.GuestName);
            if (culture == "Ja")
                model.Sqlprms[5] = new NpgsqlParameter("@kanaName", model.KanaName);
            else
                model.Sqlprms[5] = new NpgsqlParameter("@kanaName", model.GuestName);
            model.Sqlprms[6] = new NpgsqlParameter("@postalCode", model.PostalCode);
            model.Sqlprms[7] = new NpgsqlParameter("@phoneNo", model.PhoneNo);
            model.Sqlprms[8] = new NpgsqlParameter("@address1", model.Address1);
            model.Sqlprms[9] = new NpgsqlParameter("@address2", model.Address2);
            model.Sqlprms[10] = new NpgsqlParameter("@workplace", model.WorkPlace);
            model.Sqlprms[11] = new NpgsqlParameter("@nationality", model.Nationality);
            model.Sqlprms[12] = new NpgsqlParameter("@passport", model.Passport);
            model.Sqlprms[13] = new NpgsqlParameter("@sign", ConvertBase64StringToByte(model.Sign));
            model.Sqlprms[14] = new NpgsqlParameter("@guestNameimg", ConvertBase64StringToByte(model.GuestNameHW));
            if (culture == "Ja")
                model.Sqlprms[15] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.KanaNameHW));
            else
                model.Sqlprms[15] = new NpgsqlParameter("@kanaNameimg", ConvertBase64StringToByte(model.GuestNameHW));
            model.Sqlprms[16] = new NpgsqlParameter("@postalCodeimg", ConvertBase64StringToByte(model.PostalCodeHW));
            model.Sqlprms[17] = new NpgsqlParameter("@phoneNoimg", ConvertBase64StringToByte(model.PhoneNoHW));
            model.Sqlprms[18] = new NpgsqlParameter("@addressimg1", ConvertBase64StringToByte(model.AddressHW1));
            model.Sqlprms[19] = new NpgsqlParameter("@addressimg2", ConvertBase64StringToByte(model.AddressHW2));
            model.Sqlprms[20] = new NpgsqlParameter("@workPlaceimg", ConvertBase64StringToByte(model.WorkPlaceHW));
            model.Sqlprms[21] = new NpgsqlParameter("@nationalityimg", ConvertBase64StringToByte(model.NationalityHW));
            model.Sqlprms[22] = new NpgsqlParameter("@passportimg", ConvertBase64StringToByte(model.PassportHW));


            model.Sqlprms[23] = new NpgsqlParameter("@arrDate", model.ArrivalDate);
            model.Sqlprms[24] = new NpgsqlParameter("@deptDate", model.DepartureDate);
            model.Sqlprms[25] = new NpgsqlParameter("@creator", model.CreatedBy);
            model.Sqlprms[26] = new NpgsqlParameter("@updator", model.UpdatedBy);
            model.Sqlprms[27] = new NpgsqlParameter("@createddate", currentDate);
            model.Sqlprms[28] = new NpgsqlParameter("@updateddate", currentDate);

            //set file name
            string fileName = model.SystemDate.ToString("yyyyMMdd") + model.ReservationNo + model.RoomNo + DateTime.Now.ToString("yyyyMMdd") + model.HotelCode + ".jpg";
            SaveImage(model.Sign, model.HotelCode, fileName);
            model.Sqlprms[29] = new NpgsqlParameter("@signfilename", fileName);

            sql1 = "insert into trn_guestinformation(hotel_code,reservationno,roomno,systemdate,sign_filename,guestname_text,guestname_handwriting,kananame_text,kananame_handwriting,postalcode_text,postalcode_handwriting,";
            sql1 += "phoneno_text,phoneno_handwriting,address1_text,address1_handwriting,address2_text,address2_handwriting,workplace_text,workplace_handwriting,nationality_text,nationality_handwriting,";
            sql1 += "passportno_text,passportno_handwriting,arrival_date,departure_date,sign,creator,updator,created_date,updated_date)";
            sql1 += "values(@hotelcode,@reservationno,@roomno,@systemdate,@signfilename,@guestName,@guestNameimg,@kanaName,@kanaNameimg,@postalCode,@postalCodeimg,@phoneNo,@phoneNoimg,@address1,@addressimg1,@address2,@addressimg2,@workplace,@workplaceimg,@nationality,@nationalityimg,@passport,@passportimg,@arrDate,@deptDate,@sign,@creator,@updator,@createddate,@updateddate)";
            result1 = bdl.InsertUpdateDeleteData(sql1, model.Sqlprms);
            #endregion

            #region update guest information
            NpgsqlParameter[] para = new NpgsqlParameter[15];

            para[0] = new NpgsqlParameter("@guestName", model.GuestName);
            if (culture == "Ja")
                para[1] = new NpgsqlParameter("@kanaName", model.KanaName);
            else
                para[1] = new NpgsqlParameter("@kanaName", model.GuestName);
            para[2] = new NpgsqlParameter("@postalCode", model.PostalCode);
            para[3] = new NpgsqlParameter("@phoneNo", model.PhoneNo);
            para[4] = new NpgsqlParameter("@address1", model.Address1);
            para[5] = new NpgsqlParameter("@address2", model.Address2);
            para[6] = new NpgsqlParameter("@workplace", model.WorkPlace);
            para[7] = new NpgsqlParameter("@nationality", model.Nationality);
            para[8] = new NpgsqlParameter("@passport", model.Passport);            
            para[9] = new NpgsqlParameter("@arrDate", model.ArrivalDate);
            para[10] = new NpgsqlParameter("@deptDate", model.DepartureDate);
            para[11] = new NpgsqlParameter("@creator", model.CreatedBy);
            para[12] = new NpgsqlParameter("@updator", model.UpdatedBy);
            para[13] = new NpgsqlParameter("@createddate", currentDate);
            para[14] = new NpgsqlParameter("@updateddate", currentDate);
            if (result1 == "true")
            {
                sql = "update hotel_guestinformation set guestname=@guestName,kananame=@kanaName,postalcode=@postalCode,phoneno=@phoneNo,";
                sql += "address1=@address1,address2=@address2,workplace=@workplace,nationality=@nationality,passportno=@passport,";
                sql += "arrivaldate=@arrDate,departuredate=@deptDate,updator=@updator,updated_date=@updateddate";
                result2 = bdl.InsertUpdateDeleteData(sql, para);
            }            
            #endregion
            return Ok(result2);
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

        [HttpPost]
        [ActionName("GetGuestInformation")]
        public string GetGuestInformation(CardRegisterModel cardmodel)
        {
            BaseDL bdl = new BaseDL();
            string cmdText = "Select * from trn_guestinformation";
            return bdl.SelectJson(cmdText, null);
        }
    }
}
