using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commerce.IdentityManagement.Models;

namespace Commerce.IdentityManagement.Helpers
{
    public class MenuHelper
    {
        public static IEnumerable<MenuItem> GetAdminNavigationMenuItems()
        {
            var menuItems = new List<MenuItem>()
            {
                new MenuItem(){Name = "Users", DisplayText = "Users", Action = "Index", Controller = "UsersAdmin", Order = 1, IconCss = "menu-icon fa fa-users"},
                new MenuItem(){Name = "Roles", DisplayText = "Roles", Action="Index", Controller = "RolesAdmin", Order = 2, IconCss = "menu-icon fa fa-flag"}
            };

            menuItems.Insert(0, new MenuItem() { Name = "Overview", DisplayText = "Overview", Action = "Index", Controller = "Home", Order = 0, IconCss = "menu-icon fa fa-dashboard" });

            return menuItems.OrderBy(m=>m.Order).AsEnumerable();
        }
    }
}