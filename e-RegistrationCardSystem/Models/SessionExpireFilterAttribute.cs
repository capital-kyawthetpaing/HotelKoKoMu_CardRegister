using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e-RegistrationCardSystem.Models
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SessionExpireFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["CardInfo"] == null)
            {
                filterContext.Result = new RedirectResult("~/Card/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }       
    }
}