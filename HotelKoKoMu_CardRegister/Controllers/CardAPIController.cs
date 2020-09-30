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
            sql += "arrival_date=@arrDate,departure_date=@deptDate,updator=@updator,updated_date=@updateddate,sign_filename=@filename,flag='1' where hotel_code=@hotelcode and";
            sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@systemdate AS DATE) and flag='0'";

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

        /// <summary>
        /// get guest information
        /// </summary>
        /// <param name="hotelModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetGuestInformation")]
        public string GetGuestInformation(CardRegisterModel cardmodel)
        {
            BaseDL bdl = new BaseDL();
            cardmodel.Sqlprms = new NpgsqlParameter[5];
            cardmodel.Sqlprms[0] = new NpgsqlParameter("@systemid", cardmodel.HotelCode);
            cardmodel.Sqlprms[1] = new NpgsqlParameter("@pmsid", cardmodel.PMSID);
            cardmodel.Sqlprms[2] = new NpgsqlParameter("@pmspassword", cardmodel.PMSPassword);
            cardmodel.Sqlprms[3] = new NpgsqlParameter("@hotelcode", cardmodel.HotelCode);
            cardmodel.Sqlprms[4] = new NpgsqlParameter("@machineno", cardmodel.MachineNo);
            string sql = "Select hotel_code, reservationno, roomno, systemdate, guestname_hotel, kananame_hotel, postalcode_hotel, phoneno_hotel, address1_hotel, address2_hotel, workplace_hotel, nationality_hotel, passportno_hotel from trn_guestinformation";
            sql += " where flag='0' and (@pmsid isnull or pmsid=@pmsid) and (@systemid isnull or systemid=@systemid) and (@pmspassword isnull or pmspassword=@pmspassword) and (@machineno isnull or machineno=@machineno) and (@hotelcode isnull or hotel_code=@hotelcode)";
            return bdl.SelectJson(sql, cardmodel.Sqlprms);
        }
    }
}
