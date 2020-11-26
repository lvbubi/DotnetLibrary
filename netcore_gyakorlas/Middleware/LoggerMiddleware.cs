using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace netcore_gyakorlas.Middleware
{
    public class LoggerMiddleware
    {

        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(new PathString("/api")) || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }
            
            var requestBodyContent = await GetRequestBodyContent(context.Request);

            JObject json = String.IsNullOrEmpty(requestBodyContent) ? new JObject() : JObject.Parse(requestBodyContent);

            await _next(context);
        }

        private async Task<string> GetRequestBodyContent(HttpRequest request)
        {
            request.EnableBuffering();

            var bodyText = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = 0;

            return bodyText;
        }
    }
    
    
}