using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace netcore_gyakorlas.Middleware
{
    public class MinimumAgeMiddleware
    {

        private readonly RequestDelegate _next;

        public MinimumAgeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(new PathString("/api/book")))
            {
                await _next(context);
                return;
            }

            //Sad logic
            var originalBodyStream = context.Response.Body;
            var response = context.Response;
            response.Body = new MemoryStream();

            using (_next(context))
            {
                var responseBody = await GetResponseBodyContent(context.Response);
                //process and edit response
                var jToken = JContainer.Parse(responseBody);
                int dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value).Year;
            
                var filteredResult = filterResult(jToken, dateOfBirth);
            

                byte[] resultByteArray = Encoding.ASCII.GetBytes(filteredResult.ToString());

                response.ContentLength = resultByteArray.Length;
                await originalBodyStream.WriteAsync(resultByteArray);
            }
        }

        private JToken filterResult(JToken jToken, int dateOfBirth)
        {
            int userAge = DateTime.Today.Year - dateOfBirth;
            if (jToken.GetType() == typeof(JArray))
            {
                JArray books = (JArray)jToken;
                for(int i = 0; i < books.Count; i++)
                {
                    if(int.Parse(books[i]["ageLimit"].ToString()) < userAge)
                    {
                        books.RemoveAt(i);
                    }
                }

                return jToken;
            }
            else if (jToken.GetType() == typeof(JObject))
            {
                if (int.Parse(jToken["ageLimit"].ToString()) < userAge)
                {
                    return jToken;
                }
            }

            return new JObject();
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