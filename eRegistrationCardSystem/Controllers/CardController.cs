using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using eRegistrationCardSystem.Models;
using eRegistrationCardSystem.ContextDB;
using System.Data;
using Npgsql;
using System.Threading.Tasks;

namespace eRegistrationCardSystem.Controllers
{

    public class CardController : MultiLanguageController
    {
        [SessionExpireFilter]
        public ActionResult CardRegisterPage()
        {
            return View();
        }

        //for MultiLanguageChange
        public ActionResult ChangeLanguage(string key, string value)
        {
            new MultiLanguages().SetLanguage(value);
            return this.Json(new { success = true });
        }

        public ActionResult Login()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult CreateSession(string key, string value)
        {
            Session[key] = value;            
            return this.Json(new { success = true });
        }

        #region

        [HttpPost]
        [ActionName("ValidateLogin")]
        public async Task<JsonResult> ValidateLogin(LoginInfo info)
        {
            var loginStatus = new object();
            if (!checkUserStayedLogin(info))
            {
                BaseDL bdl = new BaseDL();
                AppConstants constInfo = new AppConstants();
                NpgsqlParameter[] Sqlprms = new NpgsqlParameter[4];
                Sqlprms[0] = new NpgsqlParameter("@PmsID", info.PmsID);
                Sqlprms[1] = new NpgsqlParameter("@PmsPassword", info.PmsPassword);
                Sqlprms[2] = new NpgsqlParameter("@MachineNo", info.MachineNo);
                Sqlprms[3] = new NpgsqlParameter("@HotelCode", info.HotelCode);

                if (info.SystemID == constInfo.SystemID)
                {
                    string sql_cmd = "select pmsid,pmspassword,h.hotel_code,machineno from mst_hotel h inner join mst_hotelmachine hm on h.hotel_code=hm.hotel_code where pmsid=@PmsID and pmspassword=@PmsPassword and machineno=@MachineNo and h.hotel_code=@HotelCode";
                    DataTable dt = await bdl.SelectDataTable(sql_cmd, Sqlprms);
                    if (dt.Rows.Count == 0)
                        loginStatus = CheckExistLoginInfo(info);
                    else
                    {
                        loginStatus = new
                        {
                            Status = "Success",
                            SystemID = constInfo.SystemID,
                            PmsID = dt.Rows[0]["pmsid"].ToString(),
                            PmsPassword = dt.Rows[0]["pmspassword"].ToString(),
                            HotelCode = dt.Rows[0]["hotel_code"].ToString(),
                            MachineNo = dt.Rows[0]["machineno"].ToString()
                        };
                    }
                }
                else
                    loginStatus = new { Status = "Error", Result = "SystemID is invalid" };
            }
            else
                loginStatus = new { Status = "Error", Result = "This user is stay logged in." };
            return Json(loginStatus, JsonRequestBehavior.AllowGet);
        }

        public object CheckExistLoginInfo(LoginInfo loginInfo)
        {
            BaseDL bdl = new BaseDL();
            bool status = true;
            var loginStatus = new object();
            NpgsqlParameter[] para = new NpgsqlParameter[1];
            para[0] = new NpgsqlParameter("@pmsid", loginInfo.PmsID);
            string sql = "select pmsid from mst_hotel where pmsid=@pmsid";
            DataTable dtpmsid = bdl.SelectDataTable_Info(sql, para);
            if (dtpmsid.Rows.Count == 0)
            {
                status = false;
                loginStatus = new { Status = "Error", Result = "Invalid PmsID" }; // invalid pmsid
            }
            if (status == true)
            {
                NpgsqlParameter[] para1 = new NpgsqlParameter[1];
                para1[0] = new NpgsqlParameter("@pmspassword", loginInfo.PmsPassword);
                string sql1 = "select pmspassword from mst_hotel where pmspassword=@pmspassword";
                DataTable dt = bdl.SelectDataTable_Info(sql1, para1);
                if (dt.Rows.Count == 0)
                {
                    status = false;
                    loginStatus = new { Status = "Error", Result = "Invalid PmsPassword" }; // invalid pmspassword
                }
            }
            if (status == true)
            {
                NpgsqlParameter[] para2 = new NpgsqlParameter[1];
                para2[0] = new NpgsqlParameter("@hotelcode", loginInfo.HotelCode);
                string sql2 = "select hotel_code from mst_hotel where hotel_code=@hotelcode";
                DataTable dt = bdl.SelectDataTable_Info(sql2, para2);
                if (dt.Rows.Count == 0)
                {
                    status = false;
                    loginStatus = new { Status = "Error", Result = "Invalid Hotel Code" }; // invalid pmspassword
                }
            }
            if (status == true)
            {
                NpgsqlParameter[] para3 = new NpgsqlParameter[2];
                para3[0] = new NpgsqlParameter("@hotelcode", loginInfo.HotelCode);
                para3[1] = new NpgsqlParameter("@machineno", loginInfo.MachineNo);
                string sql3 = "select machineno from mst_hotel h inner join mst_hotelmachine hm on h.hotel_code=hm.hotel_code where h.hotel_code=@hotelcode and machineno=@machineno";
                DataTable dt = bdl.SelectDataTable_Info(sql3, para3);
                if (dt.Rows.Count == 0)
                {
                    status = false;
                    loginStatus = new { Status = "Error", Result = "Invalid MachineNo" }; // invalid machine no
                }
            }
            return loginStatus;
        }

        public bool checkUserStayedLogin(LoginInfo loginInfo)
        {
            bool flag = false;
            //if(Session.Keys != null)
            //{
            //    string obj = Session["CardInfo"].ToString();
            //    if (obj != null)
            //    {
            //        string[] arr = obj.ToString().Split('_');
            //        if (arr[0] == loginInfo.SystemID
            //            && arr[1] == loginInfo.PmsID
            //            && arr[2] == loginInfo.PmsPassword
            //            && arr[3] == loginInfo.MachineNo
            //            && arr[4] == loginInfo.HotelCode
            //            )
            //            flag = true;
            //    }
            //}
            return flag;
        }
        #endregion
    }
}