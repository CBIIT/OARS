using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using OARS.Data.Static;

namespace OARS.Middleware
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseMiddleware> logger;

        public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var originalBodyStream = context.Response.Body;

            using (var responseBodyStream = new MemoryStream())
            {
                logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

                context.Response.Body = responseBodyStream;

                await _next(context);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                var responseBody = string.Empty;

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                // Log details
                logger.LogInformation("Request Path: {Path}, Response Status: {StatusCode}, Duration: {Duration} ms", context.Request.Path, context.Response.StatusCode, duration.TotalMilliseconds);

                // Copy the response body to the original stream
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
        }
    }

}