using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

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
        public ActionResult CardRegisterPage4()
        {
            return View();

        }
        public ActionResult CardRegisterPage5()
        {
            return View();

        }
        public ActionResult CardRegisterPage6()
        {
            return View();

        }
        public ActionResult CardRegisterPage7()
        {
            return View();

        }
        public ActionResult CardRegisterPage8()
        {
            return View();

        }
        public ActionResult CardRegisterPage9()
        {
            return View();

        }
        public ActionResult CardRegisterPage10()
        {
            return View();

        }
        public ActionResult CardRegisterPage11()
        {
            return View();

        }

        public ActionResult CardRegisterPage12()
        {
            return View();

        }
        public ActionResult CardRegisterPage13()
        {
            return View();

        }
        public ActionResult CardRegisterPage14()
        {
            return View();

        }
        public ActionResult CardRegisterPage15()
        {
            return View();

        }
        public ActionResult CardRegisterPage16()
        {
            return View();

        }
        public ActionResult CardRegisterPage17()
        {
            return View();

        }
        public ActionResult CardRegisterPage18()
        {
            return View();

        }
        public ActionResult CardRegisterPage19()
        {
            return View();

        }
        public ActionResult CardRegisterPage20()
        {
            return View();

        }
        public ActionResult CardRegisterPage21()
        {
            return View();

        }
        public ActionResult CardRegisterPage22()
        {
            return View();
        }

        public ActionResult RegisterPage()
        {
            return View();
        }
        public ActionResult ChangeLanguage(string key, string value)
        {
            new MultiLanguages().SetLanguage(value);
            return this.Json(new { success = true });
        }
     
    }
}