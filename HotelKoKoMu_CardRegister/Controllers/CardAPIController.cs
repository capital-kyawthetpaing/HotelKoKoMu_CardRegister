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
using System.Globalization;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace HotelKoKoMu_CardRegister.Controllers
{

    public class CardAPIController : ApiController
    {
        #region 
        /// <summary>
        /// check login for e-card system
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ValidateLogin")]
        public async Task<IHttpActionResult> ValidateLogin(LoginInfo info)
        {
            BaseDL bdl = new BaseDL();
            var loginStatus = new object();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
            Sqlprms[0] = new NpgsqlParameter("@SystemID", info.SystemID);
            Sqlprms[1] = new NpgsqlParameter("@PmsID", info.PmsID);
            Sqlprms[2] = new NpgsqlParameter("@PmsPassword", info.PmsPassword);
            Sqlprms[3] = new NpgsqlParameter("@MachineNo", info.MachineNo);
            Sqlprms[4] = new NpgsqlParameter("@HotelCode", info.HotelCode);
            string sql_cmd = "select * from trn_guestinformation where systemid=@SystemID and pmsid=@PmsID and pmspassword=@PmsPassword and machineno=@MachineNo and hotel_code=@HotelCode";
            DataTable dt = await bdl.SelectDataTable(sql_cmd, Sqlprms);
            if (dt.Rows.Count == 0)
                loginStatus = new { Result = 0 };//login failed;
            else
                loginStatus = new { Result = dt };
            return Ok(loginStatus);
        }

        /// <summary>
        /// save guest information data  from hotel system
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("requestForRegistrationCard")]
        public async Task<IHttpActionResult> requestForRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {           
            BaseDL bdl = new BaseDL();
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();            
            msgInfo = ErrorCheckforrequestRegCard(cardRegisterInfo);
            if (msgInfo.Status == "Success")
            {
                NpgsqlParameter[] para = new NpgsqlParameter[20];
                para[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                para[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                para[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                para[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                para[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
                para[5] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemDate };
                para[6] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.ReservationNo };
                para[7] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.RoomNo };
                para[8] = new NpgsqlParameter("@arrDate", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.ArriveDate };
                para[9] = new NpgsqlParameter("@deptDate", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.DepartureDate };
                para[10] = new NpgsqlParameter("@guestName", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.NameKanji };
                para[11] = new NpgsqlParameter("@kanaName", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.NameKana };
                para[12] = new NpgsqlParameter("@zipcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.ZipCode };
                para[13] = new NpgsqlParameter("@tel", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.Tel };
                para[14] = new NpgsqlParameter("@address1", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.Address1 };
                para[15] = new NpgsqlParameter("@address2", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.Address2 };
                para[16] = new NpgsqlParameter("@company", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.Company };
                para[17] = new NpgsqlParameter("@nationality", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.Nationality };
                para[18] = new NpgsqlParameter("@passport", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PassportNo };
                para[19] = new NpgsqlParameter("@createddate", NpgsqlDbType.Timestamp) { Value = DateTime.Now };
                string sql = "insert into trn_guestinformation(created_date,systemid, pmsid, pmspassword, hotel_code, machineno, systemdate, reservationno, roomno, arrivaldate_hotel, departuredate_hotel, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,flag,complete_flag) " +
                   @"values(@createddate,@SystemID, @PmsID, @PmsPassword, @hotelcode, @MachineNo, @systemdate, @reservationno, @roomno, @arrDate, @deptDate, @guestName,@kanaName, @zipcode, @tel, @address1, @address2, @company, @nationality, @passport,'0','0')";
                ReturnMessageInfo result = await bdl.InsertUpdateDeleteData(sql, para);
                if (string.IsNullOrEmpty(result.Status))
                    msgInfo = DefineError("Status");
                return Ok(msgInfo);
            }
            else
                return Ok(msgInfo);
        }

        /// <summary>
        /// get registration card data from hotel system
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("getRequestForRegistrationCard")]
        public async Task<IHttpActionResult> getRequestForRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            var cardRegistrationObj = new object();
            ReturnMessageInfo msginfo = new ReturnMessageInfo();
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
            Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
            Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
            Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
            Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
            Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
            string sql = "Select systemdate,reservationno, roomno, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,arrivaldate_hotel,departuredate_hotel from trn_guestinformation";
            sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=0 and complete_flag=0 order by created_date limit 1";
            Tuple<string, ReturnMessageInfo> result1 = await bdl.SelectJson(sql,Sqlprms);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
            if (dt.Rows.Count > 0)
            {
                NpgsqlParameter[] para = new NpgsqlParameter[5];
                para[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                para[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                para[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                para[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                para[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
                string cmdText = "update trn_guestinformation set flag = 1 where flag = 0 and complete_flag=0 and  systemid=@systemid and  pmsid=@pmsid and  pmspassword=@pmspassword and  machineno=@machineno and hotel_code=@hotelcode ";
                msginfo = await bdl.InsertUpdateDeleteData(cmdText, para);
                if (msginfo.Status == "Success")
                {
                    cardRegistrationObj = new
                    {
                        Status = "Success",
                        FailureReason = "",
                        ErrorDescription = "",
                        SystemDate = dt.Rows[0]["systemdate"].ToString(),
                        ReservationNo = dt.Rows[0]["reservationno"].ToString(),
                        RoomNo = dt.Rows[0]["roomno"].ToString(),
                        ArriveDate = dt.Rows[0]["arrivaldate_hotel"].ToString(),
                        DepartureDate = dt.Rows[0]["departuredate_hotel"].ToString(),
                        NameKanji = dt.Rows[0]["guestname_hotel"].ToString(),
                        NameKana = dt.Rows[0]["kananame_hotel"].ToString(),
                        ZipCode = dt.Rows[0]["zipcode_hotel"].ToString(),
                        Tel = dt.Rows[0]["tel_hotel"].ToString(),
                        Address1 = dt.Rows[0]["address1_hotel"].ToString(),
                        Address2 = dt.Rows[0]["address2_hotel"].ToString(),
                        Company = dt.Rows[0]["company_hotel"].ToString(),
                        Nationality = dt.Rows[0]["nationality_hotel"].ToString(),
                        PassportNo = dt.Rows[0]["passportno_hotel"].ToString()
                    };                   
                }
                else
                {
                    cardRegistrationObj = new
                    {
                        Status = msginfo.Status,
                        FailureReason = msginfo.FailureReason,
                        ErrorDescription = msginfo.ErrorDescription
                    };
                }
            }
            else
            {
                msginfo = result1.Item2;
                if (msginfo.Status == "Success")
                {
                    cardRegistrationObj = new
                    {
                        Status = "NotData",
                        FailureReason = "",
                        ErrorDescription = ""
                    };
                }
                else
                {
                    cardRegistrationObj = new
                    {
                        Status = msginfo.Status,
                        FailureReason = msginfo.FailureReason,
                        ErrorDescription = msginfo.ErrorDescription
                    };
                }
            }
            return Ok(cardRegistrationObj);
        }

        /// <summary>
        /// update Registration Card
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("setRegistrationCard")]
        public async Task<IHttpActionResult> setRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            string culture = HttpContext.Current.Request.Cookies["culture"].Value;
            DataTable dt = Get_CreatetedDate(cardRegisterInfo);
            cardRegisterInfo.CreatedDate = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString());
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[20];
            Sqlprms[0] = new NpgsqlParameter("@guestName", cardRegisterInfo.NameKanji);
            if (culture == "Ja")
                Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.NameKana);
            else
                Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.NameKanji);
            Sqlprms[2] = new NpgsqlParameter("@zipcode", cardRegisterInfo.ZipCode);
            Sqlprms[3] = new NpgsqlParameter("@tel", cardRegisterInfo.Tel);
            Sqlprms[4] = new NpgsqlParameter("@address1", cardRegisterInfo.Address1);
            Sqlprms[5] = new NpgsqlParameter("@address2", cardRegisterInfo.Address2);
            Sqlprms[6] = new NpgsqlParameter("@company", cardRegisterInfo.Company);
            Sqlprms[7] = new NpgsqlParameter("@nationality", cardRegisterInfo.Nationality);
            Sqlprms[8] = new NpgsqlParameter("@passport", cardRegisterInfo.PassportNo);
            Sqlprms[9] = new NpgsqlParameter("@updator", cardRegisterInfo.Updator);
            Sqlprms[10] = new NpgsqlParameter("@updateddate", DateTime.Now);
            string fileName = cardRegisterInfo.SystemDate + cardRegisterInfo.ReservationNo + cardRegisterInfo.RoomNo + cardRegisterInfo.CreatedDate.ToString("yyyyMMddHHmmss") + ".jpg";
            Sqlprms[11] = new NpgsqlParameter("@filename", fileName);
            Sqlprms[12] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            Sqlprms[13] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            Sqlprms[14] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
            Sqlprms[15] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);            
            Sqlprms[16] = new NpgsqlParameter("@pmsid", cardRegisterInfo.PmsID);
            Sqlprms[17] = new NpgsqlParameter("@pmspassword", cardRegisterInfo.PmsPassword);
            Sqlprms[18] = new NpgsqlParameter("@machineno", cardRegisterInfo.MachineNo);
            Sqlprms[19] = new NpgsqlParameter("@systemid", cardRegisterInfo.SystemID);
           
            string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,zipcode_text=@zipcode,tel_text=@tel,";
            sql += " address1_text=@address1,address2_text=@address2,company_text=@company,nationality_text=@nationality,passportno_text=@passport,complete_flag=1,";
            sql += " updator=@updator,updated_date=@updateddate,imagedata=@filename where pmsid=@pmsid and";
            sql += " pmspassword=@pmspassword and  machineno=@machineno and  systemid=@systemid and  hotel_code=@hotelcode and flag=1 and complete_flag=0";
            ReturnMessageInfo msgInfo = await bdl.InsertUpdateDeleteData(sql,Sqlprms);
            if(msgInfo.Status=="Success")
                SaveImage(cardRegisterInfo.ImageData, cardRegisterInfo.HotelCode, fileName);           
            return Ok(msgInfo);
        }

        [HttpPost]
        [ActionName("getRegistrationCardData")]
        public async Task<IHttpActionResult> getRegistrationCardData(CardRegisterInfo cardRegisterInfo)
        {
            GuestInformation guestinfo = new GuestInformation();                 
            BaseDL bdl = new BaseDL();
            ReturnMessageInfo msgInfo = ErrorCheck(cardRegisterInfo);
            if (msgInfo.Status == "Success")
            {
                NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
                Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };

                string sql = "Select hotel_code,created_date,systemid,pmsid,pmspassword,machineno,reservationno, roomno, systemdate, guestname_text, kananame_text, zipcode_text, tel_text, address1_text, address2_text, company_text, nationality_text, passportno_text,imagedata,flag,complete_flag from trn_guestinformation";
                sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=1";
                Tuple<string, ReturnMessageInfo> result = await bdl.SelectJson(sql, Sqlprms);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result.Item1);
                msgInfo = result.Item2;
                if (dt.Rows.Count > 0)
                {
                    msgInfo = ErrorCheckForResponse(dt,msgInfo.Status);
                    if(msgInfo.Status=="Success")
                    {
                        int flag = Convert.ToInt32(dt.Rows[0]["flag"].ToString());
                        int completeflag = Convert.ToInt32(dt.Rows[0]["complete_flag"].ToString());
                        if (flag == 1 && completeflag == 1)
                        {
                            NpgsqlParameter[] param = new NpgsqlParameter[5];
                            param[0] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["reservationno"].ToString() };
                            param[1] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["roomno"].ToString() };
                            param[2] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["systemdate"].ToString() };
                            param[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                            param[4] = new NpgsqlParameter("@createddate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString()) };
                            string sql1 = "Update trn_guestinformation set flag=2 where reservationno=@reservationno and roomno=@roomno and  systemdate=@systemdate and CAST(created_date as DATE)= CAST(@createddate AS DATE) and hotel_code=@hotelcode and flag=1 and complete_flag=1";
                            msgInfo = await bdl.InsertUpdateDeleteData(sql1, param);
                            if (!string.IsNullOrEmpty(msgInfo.Status))
                            {
                                if (msgInfo.Status == "Success")
                                {
                                    guestinfo.SystemDate = dt.Rows[0]["systemdate"].ToString();
                                    guestinfo.ReservationNo = dt.Rows[0]["reservationno"].ToString();
                                    guestinfo.RoomNo = dt.Rows[0]["roomno"].ToString();
                                    guestinfo.NameKanji = dt.Rows[0]["guestname_text"].ToString();
                                    guestinfo.NameKana = dt.Rows[0]["kananame_text"].ToString();
                                    guestinfo.ZipCode = dt.Rows[0]["zipcode_text"].ToString();
                                    guestinfo.Tel = dt.Rows[0]["tel_text"].ToString();
                                    guestinfo.Address1 = dt.Rows[0]["address1_text"].ToString();
                                    guestinfo.Address2 = dt.Rows[0]["address2_text"].ToString();
                                    guestinfo.Company = dt.Rows[0]["company_text"].ToString();
                                    guestinfo.Nationality = dt.Rows[0]["nationality_text"].ToString();
                                    guestinfo.PassportNo = dt.Rows[0]["passportno_text"].ToString();
                                    string filename = dt.Rows[0]["imagedata"].ToString();
                                    if (!String.IsNullOrWhiteSpace(filename) && filename != "")
                                        guestinfo.ImageData = CreateBase64String(filename, dt.Rows[0]["hotel_code"].ToString());
                                    guestinfo.Status = msgInfo.Status;
                                    guestinfo.FailureReason = "";
                                    guestinfo.ErrorDescription = "";
                                    return Ok(guestinfo);
                                }
                            }
                            else
                                msgInfo = DefineError("Status");
                        }
                        else
                        {
                            msgInfo.Status = "Writing";
                            msgInfo.FailureReason = "";
                            msgInfo.ErrorDescription = "";
                        }
                    }
                }
                else
                {
                    if (msgInfo.Status!= "Success")
                    {
                        msgInfo.Status = "Error";
                        msgInfo.FailureReason = msgInfo.FailureReason;
                        msgInfo.ErrorDescription = msgInfo.ErrorDescription;
                    }
                    else
                    {
                        msgInfo.Status = "NotStart";
                        msgInfo.FailureReason = "";
                        msgInfo.ErrorDescription = "";
                    }
                }
                return Ok(msgInfo);
            }
            else
                return Ok(msgInfo);
        }

        /// <summary>
        /// cancel request 
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CancelRegistrationCard")]
        public async Task<IHttpActionResult> CancelRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            var returnData = new object();
            ReturnMessageInfo msgInfo = ErrorCheck(cardRegisterInfo);
            if (msgInfo.Status == "Success")
            {
                NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
                Sqlprms[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                Sqlprms[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                Sqlprms[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                Sqlprms[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                Sqlprms[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
                string sql1 = "select systemdate,reservationno,roomno,flag,complete_flag from trn_guestinformation where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 limit 1";
                Tuple<string, ReturnMessageInfo> result1 = await bdl.SelectJson(sql1, Sqlprms);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
                msgInfo = result1.Item2;
                if (dt.Rows.Count > 0)
                {
                    msgInfo = ErrorCheckForResponse(dt,msgInfo.Status);
                    if (msgInfo.Status == "Success")
                    {
                        int flag = Convert.ToInt32(dt.Rows[0]["flag"].ToString());
                        int completeflag = Convert.ToInt32(dt.Rows[0]["complete_flag"].ToString());
                        if (flag == 1 && completeflag == 0)
                        {
                            NpgsqlParameter[] para = new NpgsqlParameter[5];
                            para[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                            para[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                            para[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                            para[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                            para[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
                            string sql2 = "Update trn_guestinformation set flag = 9 where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 and complete_flag=0";
                            msgInfo = await bdl.InsertUpdateDeleteData(sql2, para);         
                            if(!string.IsNullOrEmpty(msgInfo.Status))
                            {
                                if (msgInfo.Status == "Success")
                                {
                                    returnData = new
                                    {
                                        SystemDate = dt.Rows[0]["systemdate"].ToString(),
                                        ReservationNo = dt.Rows[0]["reservationno"].ToString(),
                                        RoomNo = dt.Rows[0]["roomno"].ToString(),
                                        Status = msgInfo.Status,
                                        FailureReason = "",
                                        ErrorDescription = ""
                                    };
                                }
                            }
                            else
                            {
                                msgInfo = DefineError("Status");
                                returnData = new
                                {
                                    Status = msgInfo.Status,
                                    FailureReason = msgInfo.FailureReason,
                                    ErrorDescription = msgInfo.ErrorDescription
                                };
                            }
                            
                        }
                    }
                    else
                    {
                        returnData = new
                        {
                            Status = msgInfo.Status,
                            FailureReason = msgInfo.FailureReason,
                            ErrorDescription = msgInfo.ErrorDescription
                        };
                    }
                }
                else
                {
                    if (msgInfo.Status != "Success")
                    {
                        returnData = new
                        {
                            Status = msgInfo.Status,
                            FailureReason = msgInfo.FailureReason,
                            ErrorDescription = msgInfo.ErrorDescription
                        };
                    }
                    else
                    {
                        returnData = new
                        {
                            Status = "NotStart",
                            FailureReason = "",
                            ErrorDescription = ""
                        };
                    }
                }
                return Ok(returnData);
            }
            else
            {
                returnData = new
                {
                    Status = msgInfo.Status,
                    FailureReason = msgInfo.FailureReason,
                    ErrorDescription = msgInfo.ErrorDescription
                };
            }
            return Ok(returnData);
        }

        public string CreateBase64String(string filename,string hotelCode)
        {
            string base64String;
            string path = WebConfigurationManager.AppSettings["imagePath"];
            var dirPath = path + hotelCode+'/'+filename;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(dirPath))
            {
                using (MemoryStream ms = new MemoryStream())
                {                    
                    image.Save(ms, image.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);                   
                }
            }
            return base64String;           
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

        public void SaveImage(string common, string HotelCode, string fileName)
        {
            string path = WebConfigurationManager.AppSettings["imagePath"];
            var dirPath = path + HotelCode;
            byte[] bytes = null;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!string.IsNullOrEmpty(common))
            {
                string[] arrCommon = common.Split(',');
                bytes = Convert.FromBase64String(arrCommon[1]);
                System.Drawing.Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                }
                dirPath = dirPath + "//" + fileName;
                image.Save(dirPath);
            }
        }

        [HttpPost]
        [ActionName("showImage")]
        public IHttpActionResult showImage(ImageInfo imageInfo)
        {            
            return Ok(CreateBase64String(imageInfo.fileName,imageInfo.HotelCode));
        }

        /// <summary>
        /// error check for requestForRegistrationCard API
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        public ReturnMessageInfo ErrorCheckforrequestRegCard(CardRegisterInfo cardRegisterInfo)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
            msgInfo.Status = "Success";
            if (string.IsNullOrEmpty(cardRegisterInfo.SystemID))
                msgInfo = DefineError("SystemID");
            else if (string.IsNullOrEmpty(cardRegisterInfo.PmsID))
                msgInfo = DefineError("PmsID");
            else if (string.IsNullOrEmpty(cardRegisterInfo.PmsPassword))
                msgInfo = DefineError("PmsPassword");
            else if (string.IsNullOrEmpty(cardRegisterInfo.HotelCode))
                msgInfo = DefineError("HotelCode");
            else if (string.IsNullOrEmpty(cardRegisterInfo.MachineNo))
                msgInfo = DefineError("MachineNo");
            else if (string.IsNullOrEmpty(cardRegisterInfo.SystemDate))
                msgInfo = DefineError("SystemDate");
            else if (string.IsNullOrEmpty(cardRegisterInfo.ReservationNo))
                msgInfo = DefineError("ReservationNo");
            else if (string.IsNullOrEmpty(cardRegisterInfo.RoomNo))
                msgInfo = DefineError("RoomNo");
            else if (string.IsNullOrEmpty(cardRegisterInfo.ArriveDate))
                msgInfo = DefineError("ArriveDate");
            else if (string.IsNullOrEmpty(cardRegisterInfo.DepartureDate))
                msgInfo = DefineError("DepartureDate");
            else if(!CheckDate(cardRegisterInfo.SystemDate))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1006";
                msgInfo.ErrorDescription = "Date check error.";
            }
            else if(!CheckDate(cardRegisterInfo.ArriveDate))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1006";
                msgInfo.ErrorDescription = "Date check error.";
            }
            else if(!CheckDate(cardRegisterInfo.DepartureDate))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1006";
                msgInfo.ErrorDescription = "Date check error.";
            }
            else if(!CheckExistForCommonRequest(cardRegisterInfo.PmsID,cardRegisterInfo.PmsPassword,cardRegisterInfo.HotelCode,cardRegisterInfo.MachineNo))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1002";
                msgInfo.ErrorDescription = "There is something wrong with Common Request and required items.";
            }                   
            else if(CheckDuplicateKey(cardRegisterInfo.HotelCode,cardRegisterInfo.ReservationNo,cardRegisterInfo.RoomNo,cardRegisterInfo.SystemDate))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1003";
                msgInfo.ErrorDescription = "Primary key is duplicate value";
            }
            return msgInfo;
        }     

        /// <summary>
        /// check error for common request is null or exist
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        public ReturnMessageInfo ErrorCheck(CardRegisterInfo cardRegisterInfo)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
            msgInfo.Status = "Success";
            if (string.IsNullOrEmpty(cardRegisterInfo.SystemID))
                msgInfo = DefineError("SystemID");
            else if (string.IsNullOrEmpty(cardRegisterInfo.PmsID))
                msgInfo = DefineError("PmsID");
            else if (string.IsNullOrEmpty(cardRegisterInfo.PmsPassword))
                msgInfo = DefineError("PmsPassword");
            else if (string.IsNullOrEmpty(cardRegisterInfo.HotelCode))
                msgInfo = DefineError("HotelCode");
            else if (string.IsNullOrEmpty(cardRegisterInfo.MachineNo))
                msgInfo = DefineError("MachineNo");
            else if (!CheckExistForCommonRequest(cardRegisterInfo.PmsID, cardRegisterInfo.PmsPassword, cardRegisterInfo.HotelCode, cardRegisterInfo.MachineNo))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1002";
                msgInfo.ErrorDescription = "There is something wrong with Common Request and required items.";
            }
            return msgInfo;
        }
                
        public ReturnMessageInfo ErrorCheckForResponse(DataTable dt,string status)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
            msgInfo.Status = "Success";
            if (string.IsNullOrEmpty(status))
                msgInfo = DefineError("Status");
            else if (string.IsNullOrEmpty(dt.Rows[0]["systemdate"].ToString()))
                msgInfo = DefineError("SystemDate");
            else if(string.IsNullOrEmpty(dt.Rows[0]["reservationno"].ToString()))
                msgInfo= DefineError("ReservationNo");
            else if (string.IsNullOrEmpty(dt.Rows[0]["roomno"].ToString()))
                msgInfo = DefineError("RoomNo");
            else if (!CheckDate(dt.Rows[0]["systemdate"].ToString()))
            {
                msgInfo.Status = "Error";
                msgInfo.FailureReason = "1006";
                msgInfo.ErrorDescription = "Date check error.";
            }
            return msgInfo;
        }

        /// <summary>
        /// define error when parameter is null
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public ReturnMessageInfo DefineError(string colName)
        {
            ReturnMessageInfo msgInfo = new ReturnMessageInfo();
            msgInfo.Status = "Error";
            msgInfo.FailureReason = "1001";
            msgInfo.ErrorDescription = colName + " is null";
            return msgInfo;
        }

        public bool CheckExistForCommonRequest(string pmsid,string pmspassword,string hotelcode,string machineno)
        {
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] para = new NpgsqlParameter[4];
            para[0] = new NpgsqlParameter("@pmsid", pmsid);
            para[1] = new NpgsqlParameter("@pmspassword", pmspassword);
            para[2] = new NpgsqlParameter("@hotelcode", hotelcode);
            para[3] = new NpgsqlParameter("@machineno", machineno);
            string sql = "select * from mst_hotel h inner join mst_hotelmachine hm on h.hotel_code=hm.hotel_code where pmsid=@pmsid and pmspassword=@pmspassword and h.hotel_code=@hotelcode and hm.machineno=@machineno";
            DataTable dt = bdl.SelectDataTable_Info(sql, para);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check when primary key is duplicate or not
        /// </summary>
        /// <param name="hCode"></param>
        /// <param name="reservNo"></param>
        /// <param name="roomNo"></param>
        /// <param name="systemDate"></param>
        /// <returns></returns>
        public bool CheckDuplicateKey(string hCode,string reservNo,string roomNo,string systemDate)
        {
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] para = new NpgsqlParameter[4];
            para[0] = new NpgsqlParameter("@hcode", hCode);
            para[1] = new NpgsqlParameter("@reservNo", reservNo);
            para[2] = new NpgsqlParameter("@roomNo", roomNo);
            para[3] = new NpgsqlParameter("@systemDate", systemDate);
            string sql = "select * from trn_guestinformation where hotel_code =@hcode and reservationno =@reservNo and roomno =@roomNo and systemdate =@systemDate";
            DataTable dt = bdl.SelectDataTable_Info(sql, para);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check parameter is date string
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool CheckDate(String date)
        {
            try
            {
                string formatString = "yyyyMMdd";
                DateTime dt = DateTime.ParseExact(date, formatString, null);               
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// get created date from trn_guestinformation 
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        public DataTable Get_CreatetedDate(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] para = new NpgsqlParameter[4];
            para[0] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
            para[1] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemDate };
            para[2] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.ReservationNo };
            para[3] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.RoomNo };
            string cmdText = "Select created_date from trn_guestinformation where hotel_code=@hotelcode and systemdate=@systemdate and reservationno = @reservationno and roomno = @roomno";
            DataTable dt = bdl.SelectDataTable_Info(cmdText, para);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            return dt;
        }

        #endregion
    }
}
