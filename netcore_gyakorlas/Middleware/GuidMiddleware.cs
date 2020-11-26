using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace netcore_gyakorlas.Middleware
{
    public class GuidMiddleware
    {

        private readonly RequestDelegate _next;

        public GuidMiddleware(RequestDelegate next)
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

            JObject json = String.IsNullOrEmpty(requestBodyContent) ? new JObject() : JObject.Parse(requestBodyContent);
 
            Guid guid = Guid.NewGuid();
            json.Add("guid", guid);
            context.Request.Body = GenerateStreamFromString(json.ToString());
            
            //Sad logic
            var originalBodyStream = context.Response.Body;
            var response = context.Response;
            response.Body = new MemoryStream();

            await _next(context);
            
            //process and edit response
            JObject resultJson = await createResponseJson(context, guid);
            byte[] resultByteArray = Encoding.ASCII.GetBytes(resultJson.ToString());

            response.ContentLength = resultByteArray.Length;
            await originalBodyStream.WriteAsync(resultByteArray);
        }

        private async Task<JObject> createResponseJson(HttpContext context, Guid guid)
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

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
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