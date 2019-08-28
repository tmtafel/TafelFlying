using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commerce.IdentityManagement.Services
{
    public enum SignInStatus
    {
        Success,
        LockedOut,
        RequiresTwoFactorAuthentication,
        Failure
    }
}