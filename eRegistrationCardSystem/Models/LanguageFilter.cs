using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRegistrationCardSystem.Models
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LanguageFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie LangCookie = HttpContext.Current.Request.Cookies["LangCookie"];
            if (LangCookie != null && LangCookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(LangCookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(LangCookie.Value);
            }
            else
            {
                HttpCookie LangCookie_ja = new HttpCookie("LangCookie");
                LangCookie_ja.Value = "ja-JP";
                LangCookie_ja.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(LangCookie_ja);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ja-JP");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja-JP");
            }
        }
    }

    public class MultiLanguages
    {
        public static List<Languages> AvailableLanguages = new List<Languages> {
            new Languages {
                LanguageFullName = "Japanese", LanguageCultureName = "ja-JP",ImageIcon="jp.png"
            },
            new Languages {
                LanguageFullName = "English", LanguageCultureName = "en-US",ImageIcon="en.png"
            },

        };
    }
    public class Languages
    {
        public string LanguageFullName
        {
            get;
            set;
        }
        public string LanguageCultureName
        {
            get;
            set;
        }
        public string ImageIcon
        {
            get;
            set;
        }
    }
}