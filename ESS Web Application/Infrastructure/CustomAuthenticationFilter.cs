using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace ESS_Web_Application.Infrastructure
{
    public class AuthorizeActionFilter : ActionFilterAttribute
    {

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //base.OnAuthorization(filterContext);

        //    if (((System.Web.Mvc.Controller)filterContext.Controller).Session["UserID"] == null)
        //    {
        //        HandleUnauthorizedRequest(filterContext);
        //    }

        //    //return;
        //}


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (((System.Web.Mvc.Controller)filterContext.Controller).Session["UserID"] == null)
            {
                //return RedirectToAction("Login", "Home");
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Account", action = "Login" }));

            }

            base.OnActionExecuting(filterContext);
        }

    }

}