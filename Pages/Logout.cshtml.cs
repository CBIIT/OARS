using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheradexPortal.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            var authenticationProperties = new OpenIdConnectChallengeProperties(
                new Dictionary<string, string?>()
                {
                    {"RedirectUri", "/" }
                }
            );
            //await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
