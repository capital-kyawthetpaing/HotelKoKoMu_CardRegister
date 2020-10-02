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
                
        /// <summary>
        /// get registration card data from hotel system
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("getRequestForRegistrationCard")]
        public IHttpActionResult getRequestForRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            var cardRegistrationObj = new object();
            string ret_Val = string.Empty;
            BaseDL bdl = new BaseDL();
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", SqlDbType.VarChar) { Value = cardRegisterInfo.SystemID };
            cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsID };
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsPassword };
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", SqlDbType.VarChar) { Value = cardRegisterInfo.HotelCode };
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", SqlDbType.VarChar) { Value = cardRegisterInfo.MachineNo };

            string sql = "Select hotel_code,created_date,systemdate,reservationno, roomno, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel from trn_guestinformation";
            sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and  machineno=@machineno and hotel_code=@hotelcode and flag=0 and complete_flag=0 order by created_date limit 1";
            Tuple<string, string> result1 = bdl.SelectJson(sql, cardRegisterInfo.Sqlprms);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
            if(dt.Rows.Count>0)
            {
                NpgsqlParameter[] para = new NpgsqlParameter[5];
                para[0] = new NpgsqlParameter("@hotelcode", SqlDbType.VarChar) { Value = cardRegisterInfo.HotelCode };
                para[1] = new NpgsqlParameter("@reservationno", SqlDbType.VarChar) { Value = dt.Rows[0]["reservationno"].ToString() };
                para[2] = new NpgsqlParameter("@roomno", SqlDbType.VarChar) { Value = dt.Rows[0]["roomno"].ToString() };
                para[3] = new NpgsqlParameter("@createddate", SqlDbType.VarChar) { Value = dt.Rows[0]["created_date"].ToString() };               
                para[4] = new NpgsqlParameter("@systemdate", SqlDbType.VarChar) { Value = dt.Rows[0]["systemdate"].ToString() };
                string cmdText = "update trn_guestinformation set flag = 1 where flag = 0 and complete_flag=0 and hotel_code = @hotelcode and reservationno = @reservationno and roomno =@roomno and CAST(created_date as Date) =CAST(@createddate as Date) and CAST(systemdate as Date) = CAST(@systemdate as Date)";
                string result2 = bdl.InsertUpdateDeleteData(cmdText, para);
                if(result2=="true")
                {
                    //card registeration data exist
                    if (dt.Rows.Count > 0 && result1.Item2 == "Success")
                        cardRegistrationObj = new { Success = result1.Item1 };
                    //card registeration data does not exist
                    else if (dt.Rows.Count == 0 && result1.Item2 == "Success")
                        cardRegistrationObj = new { NotData = "" };
                    //error
                    else
                        cardRegistrationObj = new { Error = result1.Item2 };
                }
            }
            return Ok(cardRegistrationObj);
        }

        [HttpPost]
        [ActionName("GuestInfo_Save")]
        public IHttpActionResult GuestInfo_Save(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            DateTime currentDate = DateTime.Now;
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[19];
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

            string sql = "insert into trn_guestinformation(created_date,systemid, pmsid, pmspassword, hotel_code, machineno, systemdate, reservationno, roomno, arrivaldate_hotel, departuredate_hotel, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,flag,complete_flag) " +
               @"values('2020-10-01',@SystemID, @PmsID, @PmsPassword, @hotelcode, @MachineNo, @systemdate, @reservationno, @roomno, @arrDate, @deptDate, @guestName,@kanaName, @zipcode, @tel, @address1, @address2, @company, @nationality, @PmsPassword,'0','0')";
            #endregion

            return Ok(bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms));

        }

        /// <summary>
        /// update Registration Card
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("setRegistrationCard")]
        public IHttpActionResult setRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            var returnStatus = new object();            
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[19];
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
            cardRegisterInfo.Sqlprms[9] = new NpgsqlParameter("@updator", cardRegisterInfo.Updator);
            cardRegisterInfo.Sqlprms[10] = new NpgsqlParameter("@createddate", cardRegisterInfo.CreatedDate);
            cardRegisterInfo.Sqlprms[11] = new NpgsqlParameter("@updateddate", DateTime.Now);
            string fileName = cardRegisterInfo.SystemDate.ToString("yyyyMMdd") + cardRegisterInfo.ReservationNo + cardRegisterInfo.RoomNo + DateTime.Now.ToString("yyyyMMdd") + cardRegisterInfo.HotelCode + ".jpg";
            SaveImage(cardRegisterInfo.ImageData, cardRegisterInfo.HotelCode, fileName);

            cardRegisterInfo.Sqlprms[12] = new NpgsqlParameter("@filename", fileName);
            cardRegisterInfo.Sqlprms[13] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            cardRegisterInfo.Sqlprms[14] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            cardRegisterInfo.Sqlprms[15] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
            cardRegisterInfo.Sqlprms[16] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);
            cardRegisterInfo.Sqlprms[17] = new NpgsqlParameter("@arrivaldate", cardRegisterInfo.ArrivalDate);
            cardRegisterInfo.Sqlprms[18] = new NpgsqlParameter("@departuredate", cardRegisterInfo.DepartureDate);

            string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,zipcode_text=@zipcode,tel_text=@tel,";
            sql += " address1_text=@address1,address2_text=@address2,company_text=@company,nationality_text=@nationality,passportno_text=@passport,complete_flag=1,";
            sql += " arrival_date=@arrivaldate,departure_date=@departuredate,updator=@updator,updated_date=@updateddate,imagedata=@filename where hotel_code=@hotelcode and";
            sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@createddate AS DATE) and flag=1 and complete_flag=0";
            string result = bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms);
            if (result== "true")
                returnStatus = new { Success = "Success" };            
            else
                returnStatus = new { Error =result };
            
            return Ok(returnStatus);
        }
        
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

        [HttpPost]
        [ActionName("requestForRegistrationCard")]
        public IHttpActionResult requestForRegistrationCard(CardRegisterInfo cardRegisterInfo)        
        {
            var returnStatus = new object();
            BaseDL bdl = new BaseDL();
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", SqlDbType.VarChar) { Value = cardRegisterInfo.SystemID };
            cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsID };
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsPassword };
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", SqlDbType.VarChar) { Value = cardRegisterInfo.HotelCode };
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", SqlDbType.VarChar) { Value = cardRegisterInfo.MachineNo };
            string sql = "select ";
            sql += "(case when exists ";
            sql += "(select 1 from trn_guestinformation where flag='0' and pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode) ";
            sql += "then 'Success' else 'Error'end) as Status ";
            Tuple<string, string> result = bdl.SelectJson(sql, cardRegisterInfo.Sqlprms);
            DataTable dtExistData = JsonConvert.DeserializeObject<DataTable>(result.Item1);
            if(dtExistData.Rows.Count>0)
            {
                //card registration data exist
                if (dtExistData.Rows[0][0].ToString() == "Success")
                    returnStatus = new { Success = dtExistData.Rows[0][0].ToString() };
                //error
                else
                    returnStatus = new { Error = dtExistData.Rows[0][0].ToString() };
            }
            return Ok(returnStatus);
        }

        [HttpPost]
        [ActionName("getRegistrationCardData")]
        public IHttpActionResult getRegistrationCardData(CardRegisterInfo cardRegisterInfo)
        {
            var returnStatus = new object();
            BaseDL bdl = new BaseDL();
            cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
            cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", SqlDbType.VarChar) { Value = cardRegisterInfo.SystemID };
            cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsID };
            cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", SqlDbType.VarChar) { Value = cardRegisterInfo.PmsPassword };
            cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", SqlDbType.VarChar) { Value = cardRegisterInfo.HotelCode };
            cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", SqlDbType.VarChar) { Value = cardRegisterInfo.MachineNo };

            string sql = "Select hotel_code,reservationno, roomno, systemdate, guestname_text, kananame_text, zipcode_text, tel_text, address1_text, address2_text, company_text, nationality_text, passportno_text,imagedata,flag,complete_flag from trn_guestinformation";
            sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=1";
            Tuple<string, string> result = bdl.SelectJson(sql, cardRegisterInfo.Sqlprms);

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result.Item1);
            if (dt.Rows.Count > 0)
            {
                string filename = dt.Rows[0]["sign_filename"].ToString();
                string flag = dt.Rows[0]["flag"].ToString();
                string completeflag = dt.Rows[0]["complete_flag"].ToString();
                if (!String.IsNullOrWhiteSpace(filename) && filename != "")
                {
                    var dirPath = HttpContext.Current.Server.MapPath("~/" + cardRegisterInfo.HotelCode);
                    dirPath = dirPath + "//" + filename;

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(dirPath))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            string base64String;
                            image.Save(ms, image.RawFormat);
                            byte[] imageBytes = ms.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                            dt.Rows[0]["imagedata"] = base64String;
                            string jsonstring = JsonConvert.SerializeObject(dt);
                            result = new Tuple<string, string>(jsonstring, result.Item2);
                        }
                    }
                }
                //save success , update success and return getregisterdata
                if (flag == "1" && completeflag == "1" && result.Item2 == "Success")
                    returnStatus = new { Success = result.Item1 };

                //still writing
                else if (flag == "1" && completeflag == "0" && result.Item2 == "Success")
                    returnStatus = new { Writing = "" };
                //error
                else
                    returnStatus = new { Error = result.Item2 };
            }
            else
            {
                //not yet save or update in table
                returnStatus = new { NotStart = "" };
            }

            //string sql1 = "Update trn_guestinformation set flag = 2 where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=1 and complete_flag=1";
            //string updateresult = bdl.InsertUpdateDeleteData(sql1, cardRegisterInfo.Sqlprms);
            return Ok(returnStatus);
        }


        [HttpPost]
        [ActionName("ValidateLogin")]
        public IHttpActionResult ValidateLogin(Check_Loing_Info info)
        {
            BaseDL bdl = new BaseDL();
            info.Sqlprms = new NpgsqlParameter[5];
            info.Sqlprms[0] = new NpgsqlParameter("@SystemID", info.SystemID);
            info.Sqlprms[1] = new NpgsqlParameter("@PmsID", info.PmsID);
            info.Sqlprms[2] = new NpgsqlParameter("@PmsPassword", info.PmsPassword);
            info.Sqlprms[3] = new NpgsqlParameter("@MachineNo", info.MachineNo);
            info.Sqlprms[4] = new NpgsqlParameter("@HotelCode", info.HotelCode);
            string sql_cmd = "select * from trn_guestinformation where systemid=@SystemID and pmsid=@PmsID and pmspassword=@PmsPassword and machineno=@MachineNo and hotel_code=@HotelCode";
            DataTable dt = bdl.SelectDataTable(sql_cmd, info.Sqlprms);
            return Ok(dt);
        }
    }
}
