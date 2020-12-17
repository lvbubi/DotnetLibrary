using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventApp.Models.Communication;
using Microsoft.AspNetCore.Http;
using netcore_gyakorlas.Models;
using netcore_gyakorlas.Services;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
                
                
                byte[] list = Encoding.UTF8.GetBytes("uploading...");
                await webSocket.SendAsync(new ArraySegment<byte>(list, 0, list.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

                IEnumerable<Book> bookList = books.Select(x => x.ToObject<Book>());
                
                foreach (var book in bookList)
                {
                    //_bookService.Create(book);
                    Console.WriteLine(book);
                }
                
                await SendResponse(webSocket, bookList);
                await SendCloseEvent(webSocket);
                
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }


        private async Task SendResponse(WebSocket webSocket, IEnumerable<Book> bookList)
        {
            
            byte[] list = Encoding.UTF8.GetBytes("Count: " + bookList.Count());
            await webSocket.SendAsync(new ArraySegment<byte>(list, 0, list.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            
            byte[] dataAsBytes = bookList.SelectMany(book => Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(new BookWsResponse
                {
                    Id = book.Id,
                    Author = book.AuthorId.ToString(),
                    Title = book.Title
                }) + Environment.NewLine)
            ).ToArray();
            
            await webSocket.SendAsync(new ArraySegment<byte>(dataAsBytes, 0, dataAsBytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        
        private async Task SendCloseEvent(WebSocket webSocket)
        {
            byte[] list = Encoding.UTF8.GetBytes("close");
            await webSocket.SendAsync(new ArraySegment<byte>(list, 0, list.Length), WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}