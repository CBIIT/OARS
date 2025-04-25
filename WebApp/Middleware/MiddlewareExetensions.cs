namespace OARS.Middleware
{
    public static class MiddlewareExetensions
    {
        public static IApplicationBuilder UseElapsedTimeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ElapsedTimeMiddleware>();
        }
        public static IApplicationBuilder UseLogResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogResponseMiddleware>();
        }
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
        public static IApplicationBuilder UseLog4NetTraceIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Log4NetTraceIdMiddleware>();
        }

        public static IApplicationBuilder UseUserIdHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdHeaderMiddleware>();
        }
    }
}