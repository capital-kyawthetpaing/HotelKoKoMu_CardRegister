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