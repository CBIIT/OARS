using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TheradexPortal.Data.Static;
namespace TheradexPortal.Middleware
{
    public class TheradexJsonLayout : LayoutSkeleton
    {

        static readonly string ProcessSessionId = Guid.NewGuid().ToString();
        static readonly int ProcessId = Process.GetCurrentProcess().Id;
        static readonly string MachineName = Environment.MachineName;

        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logEvent = new Dictionary<string, object>
        {
            { "timestamp", loggingEvent.TimeStamp.ToUniversalTime().ToString("o") }, // ISO 8601 format
            { "level", loggingEvent.Level.DisplayName }, // Log level (INFO, DEBUG, etc.)
            { "logger", loggingEvent.LoggerName }, // Logger name (class or namespace)
            { "thread", loggingEvent.ThreadName }, // Thread name
            { "userName", loggingEvent.LookupProperty("userName")?.ToString() ?? string.Empty }, // Custom userName property
            { "message", loggingEvent.RenderedMessage }, // The actual log message
            { "exception", loggingEvent.GetExceptionString() }, // Exception details, if any
            { "fileName", loggingEvent.LocationInformation.FileName }, // File where the log was created
            { "method", loggingEvent.LocationInformation.MethodName }, // Method where the log was created
            { "line", loggingEvent.LocationInformation.LineNumber }, // Line number in the code
            { "class", loggingEvent.LocationInformation.ClassName }, // Class name where the log was created
            { "traceId", loggingEvent.LookupProperty("traceId")?.ToString() ?? string.Empty }, // Custom traceId property
            { "identity", loggingEvent.Identity }, // Identity of the user making the log
            { "appDomain", loggingEvent.Domain }, // Application domain
            { "hostName", loggingEvent.LookupProperty("log4net:HostName")?.ToString() ?? string.Empty }, // Host machine name
            { "timestampUtc", loggingEvent.TimeStampUtc.ToString("o") }, // UTC timestamp
            { "loggerFullName", loggingEvent.LoggerName }, // Full logger name (if more detailed)
            { "customProperties", GetCustomProperties(loggingEvent) } // Include all other custom properties
        };

            // Serialize the object to JSON using System.Text.Json
            var options = new JsonSerializerOptions
            {
                WriteIndented = false, // Compact JSON format
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Camel case for JSON keys
            };

            string jsonLog = JsonConvert.SerializeObject(logEvent, Formatting.None);

            // Write the serialized JSON log to the output (file, console, etc.)
            writer.WriteLine(jsonLog);
        }

        // Additional method to capture custom properties in a more readable format
        private object GetCustomProperties(LoggingEvent loggingEvent)
        {
            var properties = loggingEvent.GetProperties();
            var result = new System.Collections.Generic.Dictionary<string, object>();

            foreach (var key in properties.GetKeys())
            {
                result[key] = properties[key];
            }

            return result;
        }

    }
    public class Log4NetTraceIdMiddleware
    {
        private const string TraceIdHeader = "X-Trace-ID";
        private const string UserHeader = "X-User-Id";
        private const string CorrelationIdHeader = "X-Correlation-ID";


        private readonly RequestDelegate _next;
        private readonly ILogger<Log4NetTraceIdMiddleware> logger;

        public Log4NetTraceIdMiddleware(RequestDelegate next, ILogger<Log4NetTraceIdMiddleware> logger)
        {
            _next = next;
            logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Set the TraceId from HttpContext.TraceIdentifier
            var traceId = context.Request.Headers[TraceIdHeader].FirstOrDefault();
            var userId = context.Request.Headers[UserHeader].FirstOrDefault();

            // Add TraceId to Log4Net's LogicalThreadContext
            log4net.LogicalThreadContext.Properties["traceId"] = traceId;
            log4net.LogicalThreadContext.Properties["userId"] = userId;

            // Log TraceId for debugging purposes
            logger.LogInformation($"Set TraceId: {traceId}");

            // Add the TraceId to the response headers
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[TraceIdHeader] = traceId;  // Add TraceId to the response header
                context.Response.Headers[UserHeader] = userId;
                return Task.CompletedTask;
            });

            // Call the next middleware
            await _next(context);

            // Optionally, clear the TraceId from LogicalThreadContext after the request
            log4net.LogicalThreadContext.Properties.Remove("traceId");
            log4net.LogicalThreadContext.Properties.Remove("userId");

        }
    }
}