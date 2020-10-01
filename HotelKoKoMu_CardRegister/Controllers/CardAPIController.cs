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
using Newtonsoft.Json;

namespace HotelKoKoMu_CardRegister.Controllers
{

    public class CardAPIController : ApiController
    {
        string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string culture = HttpContext.Current.Request.Cookies["culture"].Value;

        #region 
        /// <summary>
        /// update guest information 
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveGuestInformation")]
        public IHttpActionResult SaveGuestInformation(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            DateTime currentDate = DateTime.Now;
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[19];
            #region update guest information
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@guestName", cardRegisterInfo.GuestName);
            if (culture == "Ja")
                cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.KanaName);
            else
                cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.GuestName);
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@zipcode", cardRegisterInfo.ZipCode);
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@tel", cardRegisterInfo.Tel);
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@address1", cardRegisterInfo.Address1);
            cardRegisterInfo.Sqlprms[5] = new NpgsqlParameter("@address2", cardRegisterInfo.Address2);
            cardRegisterInfo.Sqlprms[6] = new NpgsqlParameter("@company", cardRegisterInfo.Company);
            cardRegisterInfo.Sqlprms[7] = new NpgsqlParameter("@nationality", cardRegisterInfo.Nationality);
            cardRegisterInfo.Sqlprms[8] = new NpgsqlParameter("@passport", cardRegisterInfo.Passport);
            cardRegisterInfo.Sqlprms[9] = new NpgsqlParameter("@arrDate", cardRegisterInfo.ArrivalDate);
            cardRegisterInfo.Sqlprms[10] = new NpgsqlParameter("@deptDate", cardRegisterInfo.DepartureDate);
            cardRegisterInfo.Sqlprms[11] = new NpgsqlParameter("@updator", cardRegisterInfo.Updator);
            cardRegisterInfo.Sqlprms[12] = new NpgsqlParameter("@createddate", cardRegisterInfo.CreatedDate);
            cardRegisterInfo.Sqlprms[13] = new NpgsqlParameter("@updateddate", currentDate);

            string fileName = cardRegisterInfo.SystemDate.ToString("yyyyMMdd") + cardRegisterInfo.ReservationNo + cardRegisterInfo.RoomNo + DateTime.Now.ToString("yyyyMMdd") + cardRegisterInfo.HotelCode + ".jpg";
            SaveImage(cardRegisterInfo.Sign, cardRegisterInfo.HotelCode, fileName);
            cardRegisterInfo.Sqlprms[14] = new NpgsqlParameter("@filename", fileName);

            cardRegisterInfo.Sqlprms[15] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            cardRegisterInfo.Sqlprms[16] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            cardRegisterInfo.Sqlprms[17] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
            cardRegisterInfo.Sqlprms[18] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);

            string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,zipcode_text=@zipcode,tel_text=@tel,";
            sql += "address1_text=@address1,address2_text=@address2,company_text=@company,nationality_text=@nationality,passportno_text=@passport,";
            sql += "arrival_date=@arrDate,departure_date=@deptDate,updator=@updator,updated_date=@updateddate,sign_filename=@filename,flag='1' where hotel_code=@hotelcode and";
            sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@systemdate AS DATE) and flag='0'";

            #endregion

            return Ok(bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms));
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
        /// <param name="hotelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHotelInformation")]
        public IHttpActionResult GetHotelInformation(HotelInfo hotelInfo)
        {
            BaseDL bdl = new BaseDL();
            hotelInfo.Sqlprms = new NpgsqlParameter[1];
            hotelInfo.Sqlprms[0] = new NpgsqlParameter("@hotelno", hotelInfo.HotelNo);
            string cmdText = "Select hotel_name,logo_data from mst_hotel where hotel_no=@hotelno";
            DataTable dt = bdl.SelectDataTable(cmdText, hotelInfo.Sqlprms);
            return Ok(dt);
        }

        #region 
        /// <summary>
        ///  get guest information
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetGuestInformation")]
        public IHttpActionResult GetGuestInformation(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", SqlDbType.VarChar) { Value = cardRegisterInfo.SystemID };
            cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsID };
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsPassword };
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", SqlDbType.VarChar) { Value = cardRegisterInfo.HotelCode };
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", SqlDbType.VarChar) { Value = cardRegisterInfo.MachineNo };

            string sql = "Select hotel_code, reservationno, roomno, systemdate, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel from trn_guestinformation";
            sql += " where flag='0' and (@pmsid isnull or pmsid=@pmsid) and (@systemid isnull or systemid=@systemid) and (@pmspassword isnull or pmspassword=@pmspassword) and (@machineno isnull or machineno=@machineno) and (@hotelcode isnull or hotel_code=@hotelcode)";
            return Ok(bdl.SelectJson(sql, cardRegisterInfo.Sqlprms));
        }
        #endregion
        [HttpPost]
        [ActionName("Get_HotelGuestInformation")]
        public string Get_HotelGuestInformation()
        {
            BaseDL bdl = new BaseDL();
            string sql = "Select hotel_code, reservationno, roomno, systemdate, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel from trn_guestinformation";
            return bdl.SelectJson(sql, null);
        }

        [HttpGet]
        [ActionName("requestForRegistrationCard")]
        public IHttpActionResult requestForRegistrationCard()
        {
            var cardRegistrationObj = new
            {
                Status = "Sccess",
                FailureReason= "",
                ErrorDescription= ""
            };           
            return Ok(JsonConvert.SerializeObject(cardRegistrationObj));         
        }

    }
}
