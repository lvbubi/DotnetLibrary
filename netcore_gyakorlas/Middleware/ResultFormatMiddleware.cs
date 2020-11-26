using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace netcore_gyakorlas.Middleware
{
    public class ResultFormatMiddleware
    {

        private readonly RequestDelegate _next;

        public ResultFormatMiddleware(RequestDelegate next)
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
            
            var requestBodyContent = await GetRequestBodyContent(context.Request);

            //Sad logic
            var originalBodyStream = context.Response.Body;
            var response = context.Response;
            response.Body = new MemoryStream();

            using (_next(context))
            {
                //process and edit response
                JObject resultJson = await createResponseJson(context, JObject.Parse(requestBodyContent)["guid"]?.ToString());
                byte[] resultByteArray = Encoding.ASCII.GetBytes(resultJson.ToString());

                response.ContentLength = resultByteArray.Length;
                await originalBodyStream.WriteAsync(resultByteArray);
            }
        }

        private async Task<JObject> createResponseJson(HttpContext context, String guid)
        {
            var responseBodyContent = await GetResponseBodyContent(context.Response);
            JObject resultJson = new JObject
            {
                {"content", string.IsNullOrEmpty(responseBodyContent) ? "" : JContainer.Parse(responseBodyContent)},
                {"statusCode", context.Response.StatusCode},
                {"identity", guid}
            };

            return resultJson;
        }

        private async Task<string> GetRequestBodyContent(HttpRequest request)
        {
            request.EnableBuffering();

            var bodyText = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = 0;

            return bodyText;
        }
        
        private async Task<string> GetResponseBodyContent(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string bodyText = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyText;
        }
    }
    
    
}