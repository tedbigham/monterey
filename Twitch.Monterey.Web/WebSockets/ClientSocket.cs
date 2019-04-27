using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Text;
using Twitch.Monterey.Web.Contracts;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.WebSockets
{
    public class ClientSocket
    {
        public const int MaxMessageLength = 1024;

        private readonly Guid _id = Guid.NewGuid();
        private readonly WebSocket _socket;
        private readonly RoomManager _roomManager;
        public User User { get; set; }
        private Mapper _mapper;

        public ClientSocket(WebSocket socket, HttpContext context)
        {
            _socket = socket;
            _roomManager = context.RequestServices.GetService<RoomManager>();
            _mapper = context.RequestServices.GetService<Mapper>();
        }

        public async Task ReceiveAsync()
        {
            var buffer = new byte[1024 * 4];
            while(_socket.State ==  WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                await HandleMessageAsync(result, buffer);
            }
        }

        public async Task SendMessageAsync(Message command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var message = JsonSerializer.SerializeToString(command);
            var bytes = Encoding.UTF8.GetBytes(message);
            if (bytes.Length > MaxMessageLength)
            {
                throw new InvalidOperationException($"Message larger than the maximum size of {MaxMessageLength}");
            }

            await _socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, cancellationToken);
        }

        public async Task OnConnectedAsync()
        {
            Debug.WriteLine("OnConnectedAsync");
        }

        public async Task OnCloseAsync()
        {
            foreach (var room in User.rooms)
            {
                _roomManager.GetRoom(room).UnSubscribe(this);
            }
        }

        private async Task HandleMessageAsync(WebSocketReceiveResult result, byte[] buffer)
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                try
                {
                    var json = Encoding.ASCII.GetString(buffer).TrimEnd((char)0);
                    var command = JsonSerializer.DeserializeFromString<Message>(json);

                    if (User == null && command.Op != "auth")
                    {
                        throw new Exception("not authenticated");
                    }

                    // dispatch
                    var handler = _mapper.GetHandler(command.Op);
                    var message = JsonSerializer.DeserializeFromString(json, handler.GetMessageType());
                    await handler.HandleMessage(message, this);
                    await SendMessageAsync(SuccessMessage.Instance);
                }
                catch (Exception e)
                {
                    await SendMessageAsync(new ErrorMessage { Message = e.Message});
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await OnCloseAsync();
            }
        }

        public async void OnChatMessage(ChatMessage message)
        {
            await SendMessageAsync(message);
        }

        public void OnQueryUserName(List<string> list)
        {
            // *should* always be true
            if (User != null)
            {
                list.Add(User.Name);
            }
        }
    }
}
