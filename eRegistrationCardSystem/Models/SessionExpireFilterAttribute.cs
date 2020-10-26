using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRegistrationCardSystem.Models
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SessionExpireFilterAttribute: ActionFilterAttribute
    {
        private static List<LoginInfo> loginUserList = new List<LoginInfo>();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["CardInfo"] == null)
            {
                filterContext.Result = new RedirectResult("~/Card/Login");
                return;
            }            
            base.OnActionExecuting(filterContext);
        }       
    }
}