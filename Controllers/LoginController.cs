using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheradexPortal.Data.Services;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Controllers
{

    public class LoginController : Controller
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            var redirectUri = returnUrl is null ? Url.Content("~/studies") : "/" + returnUrl;
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

        [HttpGet("signout")]
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

        [HttpGet("timeout")]
        public async Task<ActionResult> Timeout([FromQuery] string returnUrl)
        {
            //var redirectUri = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;
            var redirectUri = Url.Content("~/timedout");

            Claim userId = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "UserId").FirstOrDefault();

            if (userId != null)
                _userService.SaveActivityLog(Convert.ToInt32(userId.Value), ThorActivityType.Debug, "Entered Timeout");

            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUri);
            }

            return SignOut(new AuthenticationProperties() { RedirectUri = Url.Content("~/timedout") },
                new[]
                {
                    Okta.AspNetCore.OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                });
        }
    }
}
