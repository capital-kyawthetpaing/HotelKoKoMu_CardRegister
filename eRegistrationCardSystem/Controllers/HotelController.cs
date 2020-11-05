using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eRegistrationCardSystem.Models;
using eRegistrationCardSystem.ContextDB;
using Npgsql;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;


namespace eRegistrationCardSystem.Controllers
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
            if (Session["HotelLoginInfo"] == null)
                flag = true;
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}