using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Webapi.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally in the application
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the ErrorHandlingMiddleware
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to handle the request
        /// </summary>
        /// <param name="context">The HttpContext for the current request</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Pass the request to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception exception)
            {
                // Handle any uncaught exceptions
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Internal Server Error: {exception.Message}");
            }
        }
    }
}
