using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotelKoKoMu_CardRegister.Models;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class CardAPIController : ApiController
    {
        [HttpPost]
        [ActionName("SaveCardInformation")]
        public IHttpActionResult SaveCardInformation(CardRegisterModel model)
        {
            return Ok();
        }
    }
}
