using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Commerce.IdentityManagement.Helpers;
using Commerce.IdentityManagement.Models;

namespace Commerce.IdentityManagement.Controllers
{
    public class BaseController:Controller
    {
        private List<ActionError> _actionErrors;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _actionErrors = new List<ActionError>();
            ViewBag.ActionErrors = _actionErrors;
            ViewBag.AdminNavigationMenu = MenuHelper.GetAdminNavigationMenuItems();
            base.OnActionExecuting(filterContext);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                _actionErrors.Add(new ActionError(){Reason = error});
            }
        }

        public void AddError(Exception ex)
        {
            _actionErrors.Add(new ActionError(){Reason = ex.Message, Details = ex.StackTrace});
        }
    }
}