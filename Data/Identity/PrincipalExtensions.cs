using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Data.Identity
{
    public static class PrincipalExtensions 
    {
        public static bool IsRegistered(this ClaimsPrincipal principal)
        {
            var registeredVal = principal.FindFirstValue(ThorClaimType.Registered);
            if (registeredVal == null)
            {
                return false;
            }

            return bool.Parse(registeredVal);
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userIdVal = principal.FindFirstValue(ThorClaimType.UserId);
            if (userIdVal == null)
            {
                return 0;
            }

            return int.Parse(userIdVal);
        }

        public static bool HasTHORRole(this ClaimsPrincipal principal, string role)
        {
            return principal.HasClaim(ThorClaimType.Role, role);
        }

        public  static bool HasAnyTHORRole(this ClaimsPrincipal principal, params string[] roles)
        {
            foreach (var role in roles)
            {
                if (principal.HasTHORRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAdminRole(this ClaimsPrincipal principal, string adminRole)
        {
            return principal.HasClaim(c => c.Type == "Admin-" + adminRole);
        }

        public static bool HasAnyAdminRole(this ClaimsPrincipal principal, params string[] adminRoles)
        {
            foreach (var adminRole in adminRoles)
            {
                if (principal.HasAdminRole(adminRole))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(ThorClaimType.IsAdmin, true.ToString());
        }
    }
}
