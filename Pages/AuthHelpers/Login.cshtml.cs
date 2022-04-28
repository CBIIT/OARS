using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheradexPortal.Pages.AuthHelpers
{
    [Authorize]
    public class LoginModel : PageModel
    {
        public async Task OnGet(string? redirectUri)
        {
            var authenticationProperties = new OpenIdConnectChallengeProperties();
            authenticationProperties.RedirectUri = string.IsNullOrWhiteSpace(redirectUri) ? "/" : redirectUri;
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
        }
    }
}
