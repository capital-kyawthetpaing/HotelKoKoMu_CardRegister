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
                Status = "Success",
                FailureReason= "",
                ErrorDescription= ""
            };           
            return Ok(JsonConvert.SerializeObject(cardRegistrationObj));         
        }

        [HttpGet]
        [ActionName("getRegistrationCardData")]
        public IHttpActionResult getRegistrationCardData()
        {
            var cardRegistrationObj = new
            {
                Status = "Success",
                FailureReason = "",
                ErrorDescriptoin = "",
                SystemDate = "2020-09-30",
                ReservationNo = "00002",
                RoomNo = "01",
                NameKanji = "Mg Mg",
                NameKana = "Mg Mg",
                ZipCode = "11111",
                Tel = "012345",
                Address1 = "No35",
                Address2 = "Yangon",
                Company = "Company",
                Nationality = "Myanmar",
                PassportNo = "0123456",
                ImageData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABFEAAACCCAYAAACO/Fc1AAAbA0lEQVR4Xu3dT6xlSV0H8KrX849B5M+KuNAYFU2IgokJRsQ4iSaauBjnnfNYgbgREhOQwIpEQwyGjRj8s4C4EdiQW6ed3pg4mhgMCCQmBtENOgtgIWFhGEaG7p7Xfcs87MbXb+5795w6de49997PbKf+/OpT1T1zvjmnbgz+IUCAAAECBAgQIECAAAECBAgQWCsQ17bQgAABAgQIECBAgAABAgQIECBAIAhRHAICBAgQIECAAAECBAgQIECAQA8BIUoPJE0IECBAgAABAgQIECBAgAABAkIUZ4AAAQIECBAgQIAAAQIECBAg0ENAiNIDSRMCBAgQIECAAAECBAgQIECAgBDFGSBAgAABAgQIECBAgAABAgQI9BAQovRA0oQAAQIECBAgQIAAAQIECBAgIERxBggQIECAAAECBAgQIECAAAECPQSEKD2QNCFAgAABAgQIECBAgAABAgQICFGcAQIECBAgQIAAAQIECBAgQIBADwEhSg8kTQgQIECAAAECBAgQIECAAAECQhRngAABAgQIECBAgAABAgQIECDQQ0CI0gNJEwIECBAgQIAAAQIECBAgQICAEMUZIECAAAECBAgQIECAAAECBAj0EBCi9EDShAABAgQIECBAgAABAgQIECAgRHEGCBAgQIAAAQIECBAgQIAAAQI9BIQoPZA0IUCAAAECBAgQIECAAAECBAgIUZwBAgQIECBAgAABAgQIECBAgEAPASFKDyRNCBAgQIAAAQIECBAgQIAAAQJCFGeAAIHqAm3b5qGD5pzP+uQY40+mlJ4d2l97AgQIECBAgAABAgQITC0gRJla2PgEDkygJEC5jOgsWOm67mifCJumuRVjfHTVmnLOt7uue2yf1mstBAgQIECAAAECBPZJQIiyT7tpLQRmIFAzRDlbzj4FKU3TLGOMV/69u0/rncFxVAIBAgQIECBAgACBqgJClKqcBiNAoHaIsi9BSp8A5f7pEaT4c0SAAAECBAgQIEBgngJClHnui6oI7KzAFCHKrgcpTz755K2HH3545Sc8l2306enp7Rs3bvi0Z2f/JCicAAECBAgQIEBgHwWEKPu4q9ZEYIsCU4UouxyklJqklPwdvcWzbGoCBAgQIECAAAECFwX8D7ozQYBAVYHSwKBvEbv4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"

            };

            //Tranform it to Json object
            string json_data = JsonConvert.SerializeObject(cardRegistrationObj);
            return Ok(json_data);
        }

    }
}
