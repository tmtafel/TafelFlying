using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.IdentityManagement.Controllers
{
    public class HomeController : BaseAuhenticatedController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.AdminNavigationActiveMenu = "Overview";
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}