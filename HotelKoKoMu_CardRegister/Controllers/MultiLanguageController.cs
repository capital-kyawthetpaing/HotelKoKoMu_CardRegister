using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelKoKoMu_CardRegister.Models;

namespace HotelKoKoMu_CardRegister.Controllers
{
    public class MultiLanguageController : Controller
    {
        // GET: MultiLanguage
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = MultiLanguages.AvailableLanguages[1].LanguageCultureName;
                lang = userLanguage;

                #region comment
                //var userLanguage = Request.UserLanguages;
                //var userLang = userLanguage != null ? userLanguage[1] : "";
                //if (userLang != "")
                //{
                //    lang = userLang;
                //}
                //else
                //{
                //    lang = MultiLanguages.GetDefaultLanguage();
                //}
                #endregion
            }
            new MultiLanguages().SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }
    }
}