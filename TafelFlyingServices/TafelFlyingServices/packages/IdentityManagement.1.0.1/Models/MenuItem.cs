using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commerce.IdentityManagement.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Area { get; set; }
        public string DisplayText { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Visible { get; set; }
        public bool Authenticate { get; set; }
        public string CssClass { get; set; }
        public dynamic Data { get; set; }
        public string Roles { get; set; }
        public int Order { get; set; }
        public string AbsoluteUri { get; set; }
        public bool Active { get; set; }
        public string IconCss { get; set; }
        public IEnumerable<MenuItem> SubMenu { get; set; }
    }
}