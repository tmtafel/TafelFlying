using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commerce.IdentityManagement.Models
{
    public class LockoutViewModel
    {
        public LockoutStatus Status { get; set; }
        public DateTime? LockoutEndDate { get; set; }
    }
}