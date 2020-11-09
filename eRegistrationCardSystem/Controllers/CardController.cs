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
using NpgsqlTypes;

namespace eRegistrationCardSystem.Controllers
{
    public class CardController : Controller
    {
        [SessionExpireFilter]
        [LanguageFilter]
        public ActionResult CardRegisterPage()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SelectLanguage(string SelectedLanguage)
        {
            HttpCookie LangCookie = new HttpCookie("LangCookie");
            LangCookie.Value = SelectedLanguage;
            LangCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Response.Cookies.Add(LangCookie);            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            HttpCookie LangCookie = new HttpCookie("LangCookie");
            LangCookie.Value = "ja-JP";
            LangCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Response.Cookies.Add(LangCookie);
            return View();
        }

        [HttpPost]
        public ActionResult CreateSession(string key, string value)
        {
            Session[key] = value;          
            return this.Json(new { success = true });
        }

        [HttpGet]
        public JsonResult CheckSessionExpire()
        {
            bool flag = false;
            if (Session["CardInfo"] == null)
                flag = true;
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        

    }
}