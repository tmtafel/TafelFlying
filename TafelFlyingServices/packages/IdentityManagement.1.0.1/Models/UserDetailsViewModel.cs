using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commerce.IdentityManagement.Models
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public LockoutViewModel Lockout { get; set; }
    }
}