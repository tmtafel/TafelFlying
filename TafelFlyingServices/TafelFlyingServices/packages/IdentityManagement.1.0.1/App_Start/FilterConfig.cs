﻿using System.Web;
using System.Web.Mvc;

namespace Commerce.IdentityManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
