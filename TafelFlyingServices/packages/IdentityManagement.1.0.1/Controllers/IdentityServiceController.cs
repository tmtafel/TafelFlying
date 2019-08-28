using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Commerce.IdentityManagement.Helpers;
using Commerce.IdentityManagement.Models;
using Commerce.IdentityManagement.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Commerce.IdentityManagement.Controllers
{
    public class IdentityServiceController : ApiController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public JsonResult<IdentityUser> Users()
        {
            throw new NotImplementedException();
        }

        public async Task<ActionStatus> AccountLock(string userId, LockoutStatus status, int? forDays)
        {
            var actionStatus = new ActionStatus() {StatusCode = 500};
            try
            {
                IdentityResult result = null;
                switch (status)
                {
                    case LockoutStatus.Locked:
                        break;
                    case LockoutStatus.Unlocked:
                        break;

                }
                if (status == LockoutStatus.Locked)
                {
                    result = await UserManager.LockUserAccount(userId, forDays);
                }
                else
                {
                    result = await UserManager.LockUserAccount(userId, forDays);
                }
                if (result.Succeeded)
                {
                    actionStatus.StatusCode = 200;
                    actionStatus.Message = "Account locked";
                }
                else
                {
                    actionStatus.AddErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log this exception in logger.
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            return actionStatus;
        }
    }
}
