using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.IdentityManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BaseAuhenticatedController : BaseController
    {
	}
}