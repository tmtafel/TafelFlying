using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.IdentityManagement.Controllers
{
    public class ManageController : BaseAuhenticatedController
    {
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            return View();
        }
	}
}