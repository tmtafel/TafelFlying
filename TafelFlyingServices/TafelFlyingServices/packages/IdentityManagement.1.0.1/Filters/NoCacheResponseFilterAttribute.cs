using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.IdentityManagement.Filters
{
    public class NoCacheResponseFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.CacheControl = "no-cache";
            filterContext.HttpContext.Response.ExpiresAbsolute = DateTime.Now.AddYears(-1);
            filterContext.HttpContext.Response.Expires = -1;
            base.OnActionExecuting(filterContext);
        }
    }
}