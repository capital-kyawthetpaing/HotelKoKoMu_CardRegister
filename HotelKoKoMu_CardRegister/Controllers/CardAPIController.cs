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
        #region

        ///// <summary>
        ///// request guest information from hotel system
        ///// </summary>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionName("requestForRegistrationCard")]
        //public async Task<IHttpActionResult> requestForRegistrationCard(CardRegisterInfo info)
        //{
        //    string msg = string.Empty;
        //    BaseDL bdl = new BaseDL();
        //    info.Sqlprms = new NpgsqlParameter[5];
        //    info.Sqlprms[0] = new NpgsqlParameter("@SystemID", info.SystemID);
        //    info.Sqlprms[1] = new NpgsqlParameter("@PmsID", info.PmsID);
        //    info.Sqlprms[2] = new NpgsqlParameter("@PmsPassword", info.PmsPassword);
        //    info.Sqlprms[3] = new NpgsqlParameter("@MachineNo", info.MachineNo);
        //    info.Sqlprms[4] = new NpgsqlParameter("@HotelCode", info.HotelCode);
        //    string sql_cmd = "select * from trn_guestinformation where systemid=@SystemID and pmsid=@PmsID and pmspassword=@PmsPassword and machineno=@MachineNo and hotel_code=@HotelCode and flag=0 order by created_date limit 1";
        //    DataTable dt =await bdl.SelectDataTable(sql_cmd, info.Sqlprms);
        //    if (dt.Rows.Count > 0)
        //        msg = "success";
        //    else msg = "no record";
        //    return Ok(msg);
        //}

        ///// <summary>
        ///// get registration card data from hotel system
        ///// </summary>
        ///// <param name="cardRegisterInfo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionName("getRequestForRegistrationCard")]
        //public async Task<IHttpActionResult> getRequestForRegistrationCard(CardRegisterInfo cardRegisterInfo)
        //{
        //    var cardRegistrationObj = new object();

        //    BaseDL bdl = new BaseDL();
        //    cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
        //    cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
        //    cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
        //    cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
        //    cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //    cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };

        //    string sql = "Select hotel_code,created_date,systemdate,reservationno, roomno, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel from trn_guestinformation";
        //    sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and  machineno=@machineno and hotel_code=@hotelcode and flag=0 and complete_flag=0 order by created_date limit 1";
        //    Tuple<string, string> result1 =await bdl.SelectJson(sql, cardRegisterInfo.Sqlprms);
        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
        //    if (dt.Rows.Count > 0)
        //    {
        //        NpgsqlParameter[] para = new NpgsqlParameter[5];
        //        para[0] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //        para[1] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["reservationno"].ToString() };
        //        para[2] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["roomno"].ToString() };
        //        para[3] = new NpgsqlParameter("@createddate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString()) };
        //        para[4] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["systemdate"].ToString()) };
        //        string cmdText = "update trn_guestinformation set flag = 1 where flag = 0 and complete_flag=0 and hotel_code = @hotelcode and reservationno = @reservationno and roomno =@roomno and CAST(created_date as Date) =CAST(@createddate as Date) and CAST(systemdate as Date) = CAST(@systemdate as Date)";
        //        string result2 =await bdl.InsertUpdateDeleteData(cmdText, para);
        //        if (result2 == "true")
        //        {
        //            //card registeration data exist
        //            if (dt.Rows.Count > 0 && result1.Item2 == "Success")
        //                cardRegistrationObj = new { Success = result1.Item1 };                    
        //            //card registeration data does not exist
        //            else if (dt.Rows.Count == 0 && result1.Item2 == "Success")
        //                cardRegistrationObj = new { NotData = "" };
        //            //error
        //            else
        //                cardRegistrationObj = new { Error = result1.Item2 };
        //        }
        //    }
        //    return Ok(cardRegistrationObj);
        //}

        ///// <summary>
        ///// update Registration Card
        ///// </summary>
        ///// <param name="cardRegisterInfo"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ActionName("setRegistrationCard")]
        //public async Task<IHttpActionResult> setRegistrationCard(CardRegisterInfo cardRegisterInfo)
        //{
        //    BaseDL bdl = new BaseDL();
        //    string culture = HttpContext.Current.Request.Cookies["culture"].Value;
        //    var returnStatus = new object();
        //    cardRegisterInfo.Sqlprms = new NpgsqlParameter[19];
        //    cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@guestName", cardRegisterInfo.GuestName);
        //    if (culture == "Ja")
        //        cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.KanaName);
        //    else
        //        cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@kanaName", cardRegisterInfo.GuestName);
        //    cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@zipcode", cardRegisterInfo.ZipCode);
        //    cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@tel", cardRegisterInfo.Tel);
        //    cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@address1", cardRegisterInfo.Address1);
        //    cardRegisterInfo.Sqlprms[5] = new NpgsqlParameter("@address2", cardRegisterInfo.Address2);
        //    cardRegisterInfo.Sqlprms[6] = new NpgsqlParameter("@company", cardRegisterInfo.Company);
        //    cardRegisterInfo.Sqlprms[7] = new NpgsqlParameter("@nationality", cardRegisterInfo.Nationality);
        //    cardRegisterInfo.Sqlprms[8] = new NpgsqlParameter("@passport", cardRegisterInfo.Passport);
        //    cardRegisterInfo.Sqlprms[9] = new NpgsqlParameter("@updator", cardRegisterInfo.Updator);
        //    cardRegisterInfo.Sqlprms[10] = new NpgsqlParameter("@createddate", cardRegisterInfo.CreatedDate);
        //    cardRegisterInfo.Sqlprms[11] = new NpgsqlParameter("@updateddate", DateTime.Now);
        //    string fileName = cardRegisterInfo.SystemDate.ToString("yyyyMMdd") + cardRegisterInfo.ReservationNo + cardRegisterInfo.RoomNo + DateTime.Now.ToString("yyyyMMdd") + cardRegisterInfo.HotelCode + ".jpg";
        //    cardRegisterInfo.Sqlprms[12] = new NpgsqlParameter("@filename", fileName);
        //    cardRegisterInfo.Sqlprms[13] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
        //    cardRegisterInfo.Sqlprms[14] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
        //    cardRegisterInfo.Sqlprms[15] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
        //    cardRegisterInfo.Sqlprms[16] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);
        //    cardRegisterInfo.Sqlprms[17] = new NpgsqlParameter("@arrivaldate", cardRegisterInfo.ArrivalDate);
        //    cardRegisterInfo.Sqlprms[18] = new NpgsqlParameter("@departuredate", cardRegisterInfo.DepartureDate);

        //    string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,zipcode_text=@zipcode,tel_text=@tel,";
        //    sql += " address1_text=@address1,address2_text=@address2,company_text=@company,nationality_text=@nationality,passportno_text=@passport,complete_flag=1,";
        //    sql += " arrival_date=@arrivaldate,departure_date=@departuredate,updator=@updator,updated_date=@updateddate,imagedata=@filename where hotel_code=@hotelcode and";
        //    sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@createddate AS DATE) and flag=1 and complete_flag=0";
        //    string result =await bdl.InsertUpdateDeleteData(sql, cardRegisterInfo.Sqlprms);
        //    if (result == "true")
        //    {
        //        SaveImage(cardRegisterInfo.ImageData, cardRegisterInfo.HotelCode, fileName);
        //        returnStatus = new { Success = "Success" };
        //    }
        //    else
        //        returnStatus = new { Error = result };

        //    return Ok(returnStatus);
        //}

        //[HttpPost]
        //[ActionName("getRegistrationCardData")]
        //public async Task<IHttpActionResult> getRegistrationCardData(CardRegisterInfo cardRegisterInfo)
        //{
        //    var returnStatus = new object();
        //    BaseDL bdl = new BaseDL();
        //    cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
        //    cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
        //    cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
        //    cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
        //    cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //    cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };

        //    string sql = "Select hotel_code,created_date,systemid,pmsid,pmspassword,machineno,reservationno, roomno, systemdate, guestname_text, kananame_text, zipcode_text, tel_text, address1_text, address2_text, company_text, nationality_text, passportno_text,imagedata,flag,complete_flag from trn_guestinformation";
        //    sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=1";
        //    Tuple<string, string> result =await bdl.SelectJson(sql, cardRegisterInfo.Sqlprms);
        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result.Item1);
        //    if (dt.Rows.Count > 0)
        //    {               
        //        NpgsqlParameter[] param = new NpgsqlParameter[5];
        //        param[0] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["reservationno"].ToString() };
        //        param[1] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["roomno"].ToString() };
        //        param[2] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["systemdate"].ToString()) };
        //        param[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //        param[4] = new NpgsqlParameter("@createddate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString()) };
        //        string sql1 = "Update trn_guestinformation set flag=2 where reservationno=@reservationno and roomno=@roomno and  CAST(systemdate as DATE)=CAST(@systemdate AS DATE) and CAST(created_date as DATE)= CAST(@createddate AS DATE) and hotel_code=@hotelcode and flag=1 and complete_flag=1";
        //        string result2 =await bdl.InsertUpdateDeleteData(sql1, param);
        //        if (result2 == "true")
        //        {
        //            string filename = dt.Rows[0]["imagedata"].ToString();
        //            string flag = dt.Rows[0]["flag"].ToString();
        //            string completeflag = dt.Rows[0]["complete_flag"].ToString();
        //            if (!String.IsNullOrWhiteSpace(filename) && filename != "")
        //            {
        //                var dirPath = HttpContext.Current.Server.MapPath("~/" + cardRegisterInfo.HotelCode);
        //                dirPath = dirPath + "//" + filename;
        //                using (System.Drawing.Image image = System.Drawing.Image.FromFile(dirPath))
        //                {
        //                    using (MemoryStream ms = new MemoryStream())
        //                    {
        //                        string base64String;
        //                        image.Save(ms, image.RawFormat);
        //                        byte[] imageBytes = ms.ToArray();
        //                        base64String = Convert.ToBase64String(imageBytes);
        //                        dt.Rows[0]["imagedata"] = base64String;
        //                        string jsonstring = JsonConvert.SerializeObject(dt);
        //                        result = new Tuple<string, string>(jsonstring, result.Item2);
        //                    }
        //                }
        //            }
        //            //save success , update success and return getregisterdata
        //            if (flag == "1" && completeflag == "1" && result.Item2 == "Success")
        //                returnStatus = new { Success = result.Item1 };
        //            //still writing
        //            else if (flag == "1" && completeflag == "0" && result.Item2 == "Success")
        //                returnStatus = new { Writing = "" };
        //            //error
        //            else
        //                returnStatus = new { Error = result.Item2 };
        //        }
        //    }
        //    else 
        //        returnStatus = new { NotStart = "" };            
        //    return Ok(returnStatus);
        //}

        ///// <summary>
        ///// Convert base64String to byte array based on incoming hand writing
        ///// </summary>
        ///// <param name="common"></param>
        ///// <returns></returns>
        ///// 
        //public byte[] ConvertBase64StringToByte(string common)
        //{
        //    byte[] byteCommon = null;
        //    if (!string.IsNullOrEmpty(common))
        //    {
        //        string[] arrCommon = common.Split(',');
        //        byteCommon = Convert.FromBase64String(arrCommon[1]);
        //    }
        //    return byteCommon;
        //}

        //public void SaveImage(string common, string HotelCode, string fileName)
        //{
        //    var dirPath = HttpContext.Current.Server.MapPath("~/" + HotelCode);
        //    byte[] bytes = null;
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    if (!string.IsNullOrEmpty(common))
        //    {
        //        string[] arrCommon = common.Split(',');
        //        bytes = Convert.FromBase64String(arrCommon[1]);
        //        Image image;
        //        using (MemoryStream ms = new MemoryStream(bytes))
        //        {
        //            image = Image.FromStream(ms);
        //        }
        //        dirPath = dirPath + "//" + fileName;
        //        image.Save(dirPath);
        //    }
        //}

        //[HttpPost]
        //[ActionName("ValidateLogin")]
        //public async Task<IHttpActionResult> ValidateLogin(LoginInfo info)
        //{
        //    BaseDL bdl = new BaseDL();
        //    var loginStatus = new object();
        //    info.Sqlprms = new NpgsqlParameter[5];
        //    info.Sqlprms[0] = new NpgsqlParameter("@SystemID", info.SystemID);
        //    info.Sqlprms[1] = new NpgsqlParameter("@PmsID", info.PmsID);
        //    info.Sqlprms[2] = new NpgsqlParameter("@PmsPassword", info.PmsPassword);
        //    info.Sqlprms[3] = new NpgsqlParameter("@MachineNo", info.MachineNo);
        //    info.Sqlprms[4] = new NpgsqlParameter("@HotelCode", info.HotelCode);
        //    string sql_cmd = "select * from trn_guestinformation where systemid=@SystemID and pmsid=@PmsID and pmspassword=@PmsPassword and machineno=@MachineNo and hotel_code=@HotelCode";
        //    DataTable dt =await bdl.SelectDataTable(sql_cmd, info.Sqlprms);
        //    if (dt.Rows.Count == 0)
        //    {
        //        loginStatus = new { Result = 0 };//login failed;
        //    }
        //    else
        //    {
        //        loginStatus = new { Result = dt };
        //    }
        //    return Ok(loginStatus);
        //}

        //[HttpPost]
        //[ActionName("CancelRegistrationCard")]
        //public async Task<IHttpActionResult> CancelRegistrationCard(CardRegisterInfo cardRegisterInfo)
        //{
        //    BaseDL bdl = new BaseDL();
        //    int flag, completeflg;
        //    var returnStatus = new object();
        //    cardRegisterInfo.Sqlprms = new NpgsqlParameter[5];
        //    cardRegisterInfo.Sqlprms[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
        //    cardRegisterInfo.Sqlprms[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
        //    cardRegisterInfo.Sqlprms[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
        //    cardRegisterInfo.Sqlprms[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //    cardRegisterInfo.Sqlprms[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
        //    string sql1 = "select Cast(systemdate as Date),reservationno,roomno,flag,complete_flag from trn_guestinformation where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 limit 1";
        //    Tuple<string, string> result1 =await bdl.SelectJson(sql1, cardRegisterInfo.Sqlprms);
        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
        //    if(dt.Rows.Count>0)
        //    {
        //        NpgsqlParameter[] para = new NpgsqlParameter[5];
        //        para[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
        //        para[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
        //        para[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
        //        para[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
        //        para[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
        //        string sql2 = "Update trn_guestinformation set flag = 9 where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 and complete_flag=0";
        //        string result2 =await bdl.InsertUpdateDeleteData(sql2, para);
        //        flag = Convert.ToInt32(dt.Rows[0]["flag"].ToString());
        //        completeflg= Convert.ToInt32(dt.Rows[0]["complete_flag"].ToString());
        //        //save success , update success and return getregisterdata
        //        if (flag == 1 && completeflg == 0 && result2 == "true")
        //            returnStatus = new { Success = result1.Item1 };                
        //        //error
        //        else
        //            returnStatus = new { Error = result2 };
        //    }
        //    else
        //    {
        //        returnStatus = new { NotStart = "" };
        //    }
        //    return Ok(returnStatus);
        //}

        #endregion

        #region 

        /// <summary>
        /// request guest information from hotel system
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("requestForRegistrationCard")]
        public async Task<IHttpActionResult> requestForRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();           
            DateTime currentDate = DateTime.Now;
            
            NpgsqlParameter[] para = new NpgsqlParameter[20];           
            para[0] = new NpgsqlParameter("@SystemID", cardRegisterInfo.SystemID);
            para[1] = new NpgsqlParameter("@PmsID", cardRegisterInfo.PmsID);
            para[2] = new NpgsqlParameter("@PmsPassword", cardRegisterInfo.PmsPassword);
            para[3] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            para[4] = new NpgsqlParameter("@MachineNo", cardRegisterInfo.MachineNo);
            para[5] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);
            para[6] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            para[7] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);            
            para[8] = new NpgsqlParameter("@arrDate", cardRegisterInfo.ArriveDate);            
            para[9] = new NpgsqlParameter("@deptDate", cardRegisterInfo.DepartureDate);
            para[10] = new NpgsqlParameter("@guestName", cardRegisterInfo.NameKanji);
            para[11] = new NpgsqlParameter("@kanaName", cardRegisterInfo.NameKana);
            para[12] = new NpgsqlParameter("@zipcode", cardRegisterInfo.ZipCode);
            para[13] = new NpgsqlParameter("@tel", cardRegisterInfo.Tel);
            para[14] = new NpgsqlParameter("@address1", cardRegisterInfo.Address1);
            para[15] = new NpgsqlParameter("@address2", cardRegisterInfo.Address2);
            para[16] = new NpgsqlParameter("@company", cardRegisterInfo.Company);
            para[17] = new NpgsqlParameter("@nationality", cardRegisterInfo.Nationality);
            para[18] = new NpgsqlParameter("@passport", cardRegisterInfo.PassportNo);
            para[19] = new NpgsqlParameter("@createddate", currentDate);
            string sql = "insert into trn_guestinformation(created_date,systemid, pmsid, pmspassword, hotel_code, machineno, systemdate, reservationno, roomno, arrivaldate_hotel, departuredate_hotel, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,flag,complete_flag) " +
               @"values(@createddate,@SystemID, @PmsID, @PmsPassword, @hotelcode, @MachineNo, @systemdate, @reservationno, @roomno, @arrDate, @deptDate, @guestName,@kanaName, @zipcode, @tel, @address1, @address2, @company, @nationality, @passport,'0','0')";
            string result = await bdl.InsertUpdateDeleteData(sql, para);            
            var returnStatus = new { Status = result };          
            return Ok(returnStatus);
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
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
            Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
            Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
            Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
            Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
            Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };

            string sql = "Select hotel_code,created_date,systemdate,reservationno, roomno, guestname_hotel, kananame_hotel, zipcode_hotel, tel_hotel, address1_hotel, address2_hotel, company_hotel, nationality_hotel, passportno_hotel,arrivaldate_hotel,departuredate_hotel from trn_guestinformation";
            sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and  machineno=@machineno and hotel_code=@hotelcode and flag=0 and complete_flag=0 order by created_date limit 1";
            Tuple<string, string> result1 = await bdl.SelectJson(sql,Sqlprms);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
            if (dt.Rows.Count > 0)
            {
                NpgsqlParameter[] para = new NpgsqlParameter[5];
                para[0] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                para[1] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["reservationno"].ToString() };
                para[2] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["roomno"].ToString() };
                para[3] = new NpgsqlParameter("@createddate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString()) };
                para[4] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["systemdate"].ToString()) };
                string cmdText = "update trn_guestinformation set flag = 1 where flag = 0 and complete_flag=0 and hotel_code = @hotelcode and reservationno = @reservationno and roomno =@roomno and CAST(created_date as Date) =CAST(@createddate as Date) and CAST(systemdate as Date) = CAST(@systemdate as Date)";
                string result2 = await bdl.InsertUpdateDeleteData(cmdText, para);
                if (result2 == "true")
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
            var returnStatus = new object();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[19];
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
            Sqlprms[10] = new NpgsqlParameter("@createddate", cardRegisterInfo.CreatedDate);
            Sqlprms[11] = new NpgsqlParameter("@updateddate", DateTime.Now);
            string fileName = cardRegisterInfo.SystemDate.ToString("yyyyMMdd") + cardRegisterInfo.ReservationNo + cardRegisterInfo.RoomNo + DateTime.Now.ToString("yyyyMMdd") + cardRegisterInfo.HotelCode + ".jpg";
            Sqlprms[12] = new NpgsqlParameter("@filename", fileName);
            Sqlprms[13] = new NpgsqlParameter("@hotelcode", cardRegisterInfo.HotelCode);
            Sqlprms[14] = new NpgsqlParameter("@reservationno", cardRegisterInfo.ReservationNo);
            Sqlprms[15] = new NpgsqlParameter("@roomno", cardRegisterInfo.RoomNo);
            Sqlprms[16] = new NpgsqlParameter("@systemdate", cardRegisterInfo.SystemDate);
            Sqlprms[17] = new NpgsqlParameter("@arrivaldate", cardRegisterInfo.ArriveDate);
            Sqlprms[18] = new NpgsqlParameter("@departuredate", cardRegisterInfo.DepartureDate);
            string sql = "update trn_guestinformation set guestname_text=@guestName,kananame_text=@kanaName,zipcode_text=@zipcode,tel_text=@tel,";
            sql += " address1_text=@address1,address2_text=@address2,company_text=@company,nationality_text=@nationality,passportno_text=@passport,complete_flag=1,";
            sql += " arrival_date=@arrivaldate,departure_date=@departuredate,updator=@updator,updated_date=@updateddate,imagedata=@filename where hotel_code=@hotelcode and";
            sql += " reservationno=@reservationno and roomno=@roomno and CAST(systemdate as DATE)=CAST(@systemdate AS DATE)  and CAST(created_date as DATE)=CAST(@createddate AS DATE) and flag=1 and complete_flag=0";
            string result = await bdl.InsertUpdateDeleteData(sql,Sqlprms);
            if (result == "true")
            {
                SaveImage(cardRegisterInfo.ImageData, cardRegisterInfo.HotelCode, fileName);
                returnStatus = new { Success = "Success" };
            }
            else
                returnStatus = new { Error = result };

            return Ok(returnStatus);
        }

        [HttpPost]
        [ActionName("getRegistrationCardData")]
        public async Task<IHttpActionResult> getRegistrationCardData(CardRegisterInfo cardRegisterInfo)
        {
            var returnStatus = new object();
            BaseDL bdl = new BaseDL();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
            Sqlprms[0] = new NpgsqlParameter("@systemid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
            Sqlprms[1] = new NpgsqlParameter("@pmsid", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
            Sqlprms[2] = new NpgsqlParameter("@pmspassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
            Sqlprms[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
            Sqlprms[4] = new NpgsqlParameter("@machineno", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };

            string sql = "Select hotel_code,created_date,systemid,pmsid,pmspassword,machineno,reservationno, roomno, systemdate, guestname_text, kananame_text, zipcode_text, tel_text, address1_text, address2_text, company_text, nationality_text, passportno_text,imagedata,flag,complete_flag from trn_guestinformation";
            sql += " where pmsid=@pmsid and systemid=@systemid and  pmspassword=@pmspassword and machineno=@machineno and hotel_code=@hotelcode and flag=1";
            Tuple<string, string> result = await bdl.SelectJson(sql, Sqlprms);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result.Item1);
            if (dt.Rows.Count > 0)
            {
                NpgsqlParameter[] param = new NpgsqlParameter[5];
                param[0] = new NpgsqlParameter("@reservationno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["reservationno"].ToString() };
                param[1] = new NpgsqlParameter("@roomno", NpgsqlDbType.Varchar) { Value = dt.Rows[0]["roomno"].ToString() };
                param[2] = new NpgsqlParameter("@systemdate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["systemdate"].ToString()) };
                param[3] = new NpgsqlParameter("@hotelcode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                param[4] = new NpgsqlParameter("@createddate", NpgsqlDbType.Date) { Value = Convert.ToDateTime(dt.Rows[0]["created_date"].ToString()) };
                string sql1 = "Update trn_guestinformation set flag=2 where reservationno=@reservationno and roomno=@roomno and  CAST(systemdate as DATE)=CAST(@systemdate AS DATE) and CAST(created_date as DATE)= CAST(@createddate AS DATE) and hotel_code=@hotelcode and flag=1 and complete_flag=1";
                string result2 = await bdl.InsertUpdateDeleteData(sql1, param);
                if (result2 == "true")
                {
                    string filename = dt.Rows[0]["imagedata"].ToString();
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
            }
            else
                returnStatus = new { NotStart = "" };
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

        public void SaveImage(string common, string HotelCode, string fileName)
        {
            var dirPath = HttpContext.Current.Server.MapPath("~/" + HotelCode);
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
        /// cancel request 
        /// </summary>
        /// <param name="cardRegisterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CancelRegistrationCard")]
        public async Task<IHttpActionResult> CancelRegistrationCard(CardRegisterInfo cardRegisterInfo)
        {
            BaseDL bdl = new BaseDL();
            int flag, completeflg;
            var returnStatus = new object();
            NpgsqlParameter[] Sqlprms = new NpgsqlParameter[5];
            Sqlprms[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
            Sqlprms[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
            Sqlprms[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
            Sqlprms[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
            Sqlprms[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
            string sql1 = "select Cast(systemdate as Date),reservationno,roomno,flag,complete_flag from trn_guestinformation where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 limit 1";
            Tuple<string, string> result1 = await bdl.SelectJson(sql1, Sqlprms);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result1.Item1);
            if (dt.Rows.Count > 0)
            {
                NpgsqlParameter[] para = new NpgsqlParameter[5];
                para[0] = new NpgsqlParameter("@SystemID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.SystemID };
                para[1] = new NpgsqlParameter("@PmsID", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsID };
                para[2] = new NpgsqlParameter("@PmsPassword", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.PmsPassword };
                para[3] = new NpgsqlParameter("@HotelCode", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.HotelCode };
                para[4] = new NpgsqlParameter("@MachineNo", NpgsqlDbType.Varchar) { Value = cardRegisterInfo.MachineNo };
                string sql2 = "Update trn_guestinformation set flag = 9 where systemid = @SystemID and pmsid = @PmsID and  PmsPassword= @pmspassword and hotel_code= @HotelCode and machineno=@MachineNo and flag=1 and complete_flag=0";
                string result2 = await bdl.InsertUpdateDeleteData(sql2, para);
                flag = Convert.ToInt32(dt.Rows[0]["flag"].ToString());
                completeflg = Convert.ToInt32(dt.Rows[0]["complete_flag"].ToString());
                //save success , update success and return getregisterdata
                if (flag == 1 && completeflg == 0 && result2 == "true")
                    returnStatus = new { Success = result1.Item1 };
                //error
                else
                    returnStatus = new { Error = result2 };
            }
            else
                returnStatus = new { NotStart = "" };
            return Ok(returnStatus);
        }

        #endregion
    }
}
