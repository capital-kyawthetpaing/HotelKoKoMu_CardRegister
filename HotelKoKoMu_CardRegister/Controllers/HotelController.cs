using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Hotel_System()
        {
            return View();
        }

        public ActionResult Guest_InformationList()
        {
            return View();
        }

        public ActionResult Guest_InformationNew()
        {
            return View();
        }
    }
}