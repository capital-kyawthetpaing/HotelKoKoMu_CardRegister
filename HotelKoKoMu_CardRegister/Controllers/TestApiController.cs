using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotelKoKoMu_CardRegister.Models;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class TestApiController : ApiController
    {
        [HttpPost]
        [ActionName("GetData")]
        public IHttpActionResult GetData(CardRegisterInfo info)
        {
            return Ok("success");
        }
    }
}
