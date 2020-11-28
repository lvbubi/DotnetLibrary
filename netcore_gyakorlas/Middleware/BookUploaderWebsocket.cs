using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using netcore_gyakorlas.Models;
using netcore_gyakorlas.Services;
using Newtonsoft.Json.Linq;

namespace netcore_gyakorlas.Middleware
{
    public class BookUploaderWebsocket
    {
        private readonly RequestDelegate _next;

        public BookUploaderWebsocket(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBookService bookService)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await Echo(webSocket);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }
        
        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var mes = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (result.Count == 0)
                {
                    throw new WebSocketException();
                }

                JArray books = JArray.Parse(mes);
                
                byte[] list = Encoding.UTF8.GetBytes("punci");
                await webSocket.SendAsync(new ArraySegment<byte>(list, 0, list.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                
                foreach (var jToken in books)
                {
                    Book book = jToken.ToObject<Book>();
                    //_bookService.Create(book);
                    Console.WriteLine(book);
                }

                //books.Count;

                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}