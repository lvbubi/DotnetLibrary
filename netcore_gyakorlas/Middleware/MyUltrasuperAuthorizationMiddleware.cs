using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace netcore_gyakorlas.Middleware
{
    public class MyUltrasuperAuthorizationMiddleware
    {

        private readonly RequestDelegate _next;

        public MyUltrasuperAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(new PathString("/api")))
            {
                await _next(context);
                return;
            }

            if (context.Request.Method != "GET" && context.User.IsInRole("User"))
            {
                context.Response.StatusCode = 403;
                throw new UnauthorizedAccessException();
            }

            await _next(context);
        }
    }
}