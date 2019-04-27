using System;
using System.Threading.Tasks;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace Twitch.Monterey.Web.Contracts
{
    public class CreateRoomHandler : MessageHandler<RoomMessage>
    {
        private readonly RoomManager _roomManager;

        public CreateRoomHandler(IServiceProvider services)
        {
            _roomManager = services.GetService<RoomManager>();
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            if (string.IsNullOrEmpty(command.Room))
            {
                throw new Exception("missing: room");
            }
            _roomManager.CreateRoom(command.Room, socket.User.Name);
            //JoinRoom(command);
        }
    }
}
