using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using OARS.Data.Static;

namespace OARS.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string TraceIdHeader = "X-Trace-ID";
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString();
            var correlationId = Guid.NewGuid().ToString();


            context.Response.Headers[TraceIdHeader] = traceId;
            context.Response.Headers[CorrelationIdHeader] = correlationId;

            context.Request.Headers[TraceIdHeader] = traceId;
            context.Request.Headers[CorrelationIdHeader] = correlationId;

            await _next(context);
        }
    }

}