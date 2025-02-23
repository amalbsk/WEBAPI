using Serilog;

namespace Webapi.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the RequestLoggingMiddleware
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<RequestLoggingMiddleware>();
        }

        /// <summary>
        /// Invokes the middleware to log request details
        /// </summary>
        /// <param name="context">The HttpContext for the current request</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware in the pipeline
                await _next(context);
            }
            finally
            {
                // Log request details regardless of the outcome
                _logger.Information(
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode);
            }
        }
    }
}
