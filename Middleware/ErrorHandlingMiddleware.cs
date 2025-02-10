using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Webapi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Internal Server Error: {exception.Message}");
            }
        }
    }
}
