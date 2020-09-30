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
        // GET: Card
        public ActionResult CardRegisterPage1()
        {
            return View();
        }
        public ActionResult CardRegisterPage2()
        {
            return View();
        }

        public ActionResult CardRegisterPage3()
        {
            return View();

        }
        public ActionResult CardRegisterPage21(CardRegisterModel cardModel)
        {            
            return View(cardModel);
        }
        public ActionResult CardRegisterPage22()
        {
            return View();
        }

        public ActionResult RegisterPage(CardRegisterModel cardModel)
        {
            return View(cardModel);
        }
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

       

    }
}