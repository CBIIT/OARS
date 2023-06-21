using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TheradexPortal.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("Login")]
        public IActionResult Login([FromQuery] string returnUrl)
        {
            var redirectUri = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                // Get and store the login to pull the email address
                Claim emailClaim = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "preferred_username").FirstOrDefault();
                
                return LocalRedirect(redirectUri);
            }

            //return null;
            return Challenge();
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> Logout([FromQuery] string returnUrl)
        {
            var redirectUri = returnUrl is null ? Url.Content("~/") : "/" + returnUrl;

            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUri);
            }
            Response.Redirect("https://theradexbeta.oktapreview.com/logout");

            await HttpContext.SignOutAsync();

            return LocalRedirect(redirectUri);
        }
    }
}
