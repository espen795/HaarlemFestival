using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaarlemFestival.Models
{
    public class AdminAuthorize : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Als de gebruiker zelf naar de loginpagina navigeert.
            if (HttpContext.Current.Request.Url.AbsolutePath == "/Admin/Login")
                return;
;
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.Current.Session["NotLoggedIn"] = true;
                return;
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                HttpContext.Current.Session["NotAuthorized"] = true;
                filterContext.Result = new RedirectResult("~/Admin/Overview");
                return;
            }
        }
    }
}