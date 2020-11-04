using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace eRegistrationCardSystem.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LocsAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // If the browser session has expired...
            if (ctx.Session["CardInfo"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary {
                        { "Controller", "Card" },
                        { "Action", "Login" }
                   });

                //if (filterContext.HttpContext.Request.IsAjaxRequest())
                //{
                //    // For AJAX requests, we're overriding the returned JSON result with a simple string,
                //    // indicating to the calling JavaScript code that a redirect should be performed.
                //    filterContext.Result = new JsonResult { Data = "_Logon_" };
                //}
                //else
                //{
                //    // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                //    // simply displays a temporary 5 second notification that they have timed out, and
                //    // will, in turn, redirect to the logon page.
                //    filterContext.Result = new RedirectToRouteResult(
                //        new RouteValueDictionary {
                //        { "Controller", "Home" },
                //        { "Action", "TimeoutRedirect" }
                //    });
                //}
            }            
        }
    }
}
