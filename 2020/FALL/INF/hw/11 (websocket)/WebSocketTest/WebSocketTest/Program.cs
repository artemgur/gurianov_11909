using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebTest
{
    public static class Program
    {
        private static List<WebSocket> webSockets;
        
        public static async Task Main(string[] args)
        {
            webSockets = new List<WebSocket>();
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine($"WebServer started!");

            await Task.Run(async () =>
            {
                while (true)
                {
                    var ctx = await listener.GetContextAsync();
                    if (ctx.Request.IsWebSocketRequest)
                        await Task.Run(() => WebSocketHandle(ctx));
                }
            });
        }

        public async static void WebSocketHandle(HttpListenerContext context)
        {
            WebSocketContext webSocketContext;
            try
            {
                webSocketContext = await context.AcceptWebSocketAsync(null);
                var ipAddress = context.Request.UserHostAddress.ToString();
                Console.WriteLine($"Connected: {ipAddress}");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.Close();
                Console.WriteLine($"Exception: {e}");
                return;
            }
            try
            {
                using var webSocket = webSocketContext.WebSocket;
                webSockets.Add(webSocket);
                var cts = new CancellationTokenSource();
                while (webSocket.State == WebSocketState.Open)
                {                        
                    var receiveBuffer = new byte[1024];
                    var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    else
                    {
                        var length = receiveBuffer.TakeWhile(b => b != 0).Count();
                        var result = Encoding.UTF8.GetString(receiveBuffer, 0, length);
                        foreach (var socket in webSockets)
                            await SendMessage(result, socket);
                    }
                }
                cts.Cancel();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
            finally
            {
                Console.WriteLine($"Disconnected");
            }
        }

        public static async Task SendMessage(string msg, WebSocket webSocket)
        {
            var output = Encoding.UTF8.GetBytes(msg);
            await webSocket.SendAsync(new ArraySegment<byte>(output, 0, msg.Length),
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}