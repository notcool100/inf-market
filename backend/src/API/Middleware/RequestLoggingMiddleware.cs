using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InfluencerMarketplace.API.Middleware
{
    /// <summary>
    /// Request logging middleware for tracking HTTP requests and responses
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            // Log request details
            var requestBody = await ReadRequestBodyAsync(request);
            var requestPath = request.Path;
            var requestMethod = request.Method;
            var requestQuery = request.QueryString.ToString();

            _logger.LogInformation(
                "Request: {Method} {Path}{Query} | Body: {RequestBody}",
                requestMethod,
                requestPath,
                requestQuery,
                SanitizeSensitiveData(requestBody));

            // Capture response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var responseBodyContent = await ReadResponseBodyAsync(context.Response);
                await responseBody.CopyToAsync(originalBodyStream);

                _logger.LogInformation(
                    "Response: {Method} {Path} | Status: {StatusCode} | Duration: {ElapsedMilliseconds}ms | Body: {ResponseBody}",
                    requestMethod,
                    requestPath,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    SanitizeSensitiveData(responseBodyContent));
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength ?? 0)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            request.Body.Position = 0;
            return Encoding.UTF8.GetString(buffer);
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private string SanitizeSensitiveData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;

            var sensitiveFields = new[] { "password", "token", "authorization", "secret", "apiKey" };
            var sanitized = data;

            foreach (var field in sensitiveFields)
            {
                // Simple regex-like replacement for JSON fields
                var pattern = $@"""{field}""\s*:\s*""[^""]*""";
                sanitized = System.Text.RegularExpressions.Regex.Replace(
                    sanitized,
                    pattern,
                    $"\"{field}\":\"***REDACTED***\"",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            return sanitized;
        }
    }
}

