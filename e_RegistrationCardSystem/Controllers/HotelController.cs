using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e_RegistrationCardSystem.Models;
using e_RegistrationCardSystem.ContextDB;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using System.IO;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using ActionNameAttribute = System.Web.Mvc.ActionNameAttribute;
using System.Web.Http.Results;

namespace e_RegistrationCardSystem.Controllers
{
    public class HotelController : Controller
    {
        public ActionResult HotelLogin()
        {
            return View();
        }

        public ActionResult HotelGuestList()
        {
            if (Session["HotelLoginInfo"] == null)
                return RedirectToAction("HotelLogin", "Hotel");
            return View();
        }

        public ActionResult GuestInformationList()
        {
            if (Session["CardInfo"] == null)            
                return RedirectToAction("Login", "Card");           
            return View();
        }

        public ActionResult GuestInformationNew()
        {
            if (Session["HotelLoginInfo"] == null)
                return RedirectToAction("HotelLogin", "Hotel");
            return View();
        }
    }
}