using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using OARS.Data.Static;
namespace OARS.Middleware
{
    //public class RequestLoggingMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILogger<RequestLoggingMiddleware> _logger;

    //    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    //    {
    //        _next = next;
    //        _logger = logger;
    //    }

    //    public async Task InvokeAsync(HttpContext context)
    //    {
    //        _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

    //        //foreach (var header in context.Request.Headers)
    //        //{
    //        //    _logger.LogInformation("TraceId: {TraceId}, Request: {Method} {Path}  Header: {Key}: {Value}", traceId, context.Request.Method, context.Request.Path, header.Key, header.Value);
    //        //}

    //        await _next(context);
    //    }
    //}

    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Copy pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            // Get incoming request
            var request = await GetRequestAsTextAsync(context.Request);
            _logger.LogInformation(request);

            // Create a new memory stream and use it for the temp response body
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue down the Middleware pipeline
            await _next(context);

            // Format the response from the server
            var response = await GetResponseAsTextAsync(context.Response);
            var trimmedResponse = TrimHtmlIfNeeded(response);
            _logger.LogInformation(trimmedResponse);

            // Copy the contents of the new memory stream, which contains the response to the original stream,
            // which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            var body = request.Body;

            // Set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            // Read request stream
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            // Convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            // Assign the read body back to the request body
            request.Body.Position = 0;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            // Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
        // Method to trim only the HTML content if present
        private string TrimHtmlIfNeeded(string content)
        {
            const int maxLength = 2000;

            // Check if content contains HTML tags
            if (content.Contains("<html", StringComparison.OrdinalIgnoreCase) || content.Contains("<!DOCTYPE html", StringComparison.OrdinalIgnoreCase))
            {
                // If content is HTML, trim only the HTML part
                return content.Length > maxLength ? content.Substring(0, maxLength) + "..." : content;
            }

            // Return original content if no HTML is found
            return content;
        }
    }

}