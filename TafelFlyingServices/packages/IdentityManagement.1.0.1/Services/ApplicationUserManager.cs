﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Commerce.IdentityManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Commerce.IdentityManagement.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            var identitySettings = new IdentitySettings();
            //TODO: Implement reading these identity setting from a store.

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = identitySettings.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = identitySettings.RequireUniqueEmail
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = identitySettings.MinimumPasswordLength,
                RequireNonLetterOrDigit = identitySettings.RequireNonLetterOrDigit,
                RequireDigit = identitySettings.RequireDigit,
                RequireLowercase = identitySettings.RequireLowercase,
                RequireUppercase = identitySettings.RequireUppercase,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = identitySettings.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = identitySettings.DefaultAccountLockoutTimeSpan;
            manager.MaxFailedAccessAttemptsBeforeLockout = identitySettings.MaxFailedAccessAttemptsBeforeLockout;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        /// <summary>
        /// Method to add user to multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> AddUserToRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Add user to each role using UserRoleStore
            foreach (var role in roles.Where(role => !userRoles.Contains(role)))
            {
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);
            }

            // Call update once when all roles are added
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove user from multiple roles
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="roles">list of role names</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user Id");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            // Remove user to each role using UserRoleStore
            foreach (var role in roles.Where(userRoles.Contains))
            {
                await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
            }

            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> LockUserAccount(string userId, int? forDays)
        {
            var result = await this.SetLockoutEnabledAsync(userId, true);
            if (result.Succeeded)
            {
                if (forDays.HasValue)
                {
                    result = await SetLockoutEndDateAsync(userId, DateTimeOffset.UtcNow.AddDays(forDays.Value));
                }
                else
                {
                    result = await SetLockoutEndDateAsync(userId, DateTimeOffset.MaxValue);
                }
            }
            return result;
        }

        public virtual async Task<IdentityResult> UnlockUserAccount(string userId)
        {
            var result = await this.SetLockoutEnabledAsync(userId, false);
            if (result.Succeeded)
            {
                await ResetAccessFailedCountAsync(userId);
            }
            return result;
        }
    }
}