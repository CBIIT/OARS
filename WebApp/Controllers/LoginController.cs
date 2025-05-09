﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Security.Claims;
using OARS.Data;
using OARS.Data.Services;
using OARS.Data.Services.Abstract;
using OARS.Data.Static;
using OARS.Data.Models;

namespace OARS.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;
        private IHttpContextAccessor _contextAccessor;  

        public LoginController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _contextAccessor = httpContextAccessor;
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

                // Get first page to navigate to
                string dashboards = _contextAccessor.HttpContext.User.FindFirst(ThorClaimType.Dashboards).Value;
                string firstDash = dashboards.Trim('|').Split('|')[0];
                redirectUri = returnUrl is null ? Url.Content($"{ThorConstants.DASHBOARD_PAGE_PATH}/{firstDash}") : "/" + returnUrl;

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

            Claim userId = ((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "UserId").FirstOrDefault();
            // Determine if user is CTEP or Theradex
            User curUser = await _userService.GetUserAsync(Convert.ToInt32(userId.Value));

            string returnType = "";
            if (curUser != null && !curUser.IsCtepUser)
                returnType = "internallogin";

            return SignOut(new AuthenticationProperties() { RedirectUri = Url.Content("~/" + returnType) },
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
            // Determine if user is CTEP or Theradex
            User curUser = await _userService.GetUserAsync(Convert.ToInt32(userId.Value));

            if (userId != null)
                _userService.SaveActivityLog(Convert.ToInt32(userId.Value), ThorActivityType.Logout, "Timeout");

            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect(redirectUri);
            }

            string returnType = "";
            if (!curUser.IsCtepUser)
                returnType = "/internal";
            return SignOut(new AuthenticationProperties() { RedirectUri = Url.Content("~/timedout" + returnType) },
                new[]
                {
                    Okta.AspNetCore.OktaDefaults.MvcAuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                });
        }
    }
}
