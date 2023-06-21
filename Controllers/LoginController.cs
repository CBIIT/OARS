using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TheradexPortal.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            var redirectUri = returnUrl is null ? Url.Content("~/Studies") : "/" + returnUrl;
            //var redirectUri = returnUrl is null ? Url.Content("~/") : "/Studies";

            if (User.Identity.IsAuthenticated)
            {
                // Get and store the login to pull the email address
                Claim emailClaim = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "preferred_username").FirstOrDefault();
                
                return LocalRedirect(redirectUri);
            }

            //return null;
            return Challenge();
        }

        [HttpGet("Signout")]
        public async Task<ActionResult> Signout([FromQuery] string returnUrl)
        {
            var redirectUri = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;

            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUri);
            }

            return SignOut(new AuthenticationProperties() { RedirectUri = Url.Content("~/") },
                new[]
                {
                    Okta.AspNetCore.OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                });
        }
    }
}
