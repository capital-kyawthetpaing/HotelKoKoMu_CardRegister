﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eRegistrationCardSystem.Models;

namespace eRegistrationCardSystem.Controllers
{
    public class MultiLanguageController : Controller
    {
        //// GET: MultiLanguage
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    string lang = null;
        //    HttpCookie langCookie = Request.Cookies["culture"];
        //    if (langCookie != null)
        //    {
        //        lang = langCookie.Value;
        //    }
        //    else
        //    {
        //        var userLanguage = MultiLanguages.AvailableLanguages[1].LanguageCultureName;
        //        //var userLanguage = MultiLanguages.AvailableLanguages[0].LanguageCultureName;
        //        lang = userLanguage;

        //        #region comment
        //        //var userLanguage = Request.UserLanguages;
        //        //var userLang = userLanguage != null ? userLanguage[1] : "";
        //        //if (userLang != "")
        //        //{
        //        //    lang = userLang;
        //        //}
        //        //else
        //        //{
        //        //    lang = MultiLanguages.GetDefaultLanguage();
        //        //}
        //        #endregion
        //    }
        //    new MultiLanguages().SetLanguage(lang);
        //    return base.BeginExecuteCore(callback, state);
        //}


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
            }
            new MultiLanguages().SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }
    }
}