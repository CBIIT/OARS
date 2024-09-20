using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Middleware
{
    public class UserIdHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                // Retrieve the UserId (or any claim like name, email, etc.)
                var userId = context.User.FindFirst("sub")?.Value; // "sub" is typically used for UserId in OpenID Connect

                // Add UserId to the request header
                if (!string.IsNullOrEmpty(userId))
                {
                    context.Request.Headers["X-UserId"] = userId;

                    // Add UserId to the response header
                    context.Response.OnStarting(() =>
                    {
                        context.Response.Headers["X-UserId"] = userId;
                        return Task.CompletedTask;
                    });
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

}