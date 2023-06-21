using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Data.Identity
{
    public static class PrincipalExtensions 
    {
        public static bool IsRegistered(this ClaimsPrincipal principal)
        {
            var registeredVal = principal.FindFirstValue(WRClaimType.Registered);
            if (registeredVal == null)
            {
                return false;
            }

            return bool.Parse(registeredVal);
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userIdVal = principal.FindFirstValue(WRClaimType.UserId);
            if (userIdVal == null)
            {
                return 0;
            }

            return int.Parse(userIdVal);
        }

        public static bool HasWrRole(this ClaimsPrincipal principal, string role)
        {
            return principal.HasClaim(WRClaimType.Role, role);
        }

        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(WRClaimType.IsAdmin, true.ToString());
        }
    }
}
