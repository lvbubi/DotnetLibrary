using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
            JObject json = JObject.Parse(requestBodyContent);
            json.Add("guid", "random");
            byte[] byteArray = Encoding.ASCII.GetBytes(json.ToString());
            MemoryStream stream = new MemoryStream( byteArray ); 
            context.Request.Body = stream;
            
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                var response = context.Response;
                response.Body = responseBody;

                await _next(context);

                var responseBodyContent = await GetResponseBodyContent(context.Response);

                response.ContentLength = byteArray.Length;
                await originalBodyStream.WriteAsync(byteArray);

                //await responseBody.CopyToAsync(originalBodyStream);
            }
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