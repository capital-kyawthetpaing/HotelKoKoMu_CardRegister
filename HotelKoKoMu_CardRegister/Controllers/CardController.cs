using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HotelKoKoMu_CardRegister.Models;


namespace HotelKoKoMu_CardRegister.Controllers
{

    public class CardController : MultiLanguageController
    {
       //[SessionExpireFilter]
        public ActionResult CardRegisterPage()
        {
            //if(Session["CardInfo"] ==null)
            //{
            //    return RedirectToAction("Login", "Card");
            //}
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
    }
}