using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class ChatMessageHandler : MessageHandler<ChatMessage>
    {
        private readonly RoomManager _roomManager;

        public ChatMessageHandler(IServiceProvider services)
        {
            _roomManager = services.GetService<RoomManager>();
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var chat = (ChatMessage)message;
            if (!socket.User.rooms.Contains(chat.Room))
            {
                throw new Exception("not joined");
            }

            chat.Sender = socket.User.Name;
            _roomManager.GetRoom(chat.Room).Broadcast(chat);
        }
    }
}
