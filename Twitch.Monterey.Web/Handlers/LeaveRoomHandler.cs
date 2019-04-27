using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class LeaveRoomHandler : MessageHandler<RoomMessage>
    {
        private readonly RoomManager _roomManager;
        private readonly UserDatabase _userDatabase;

        public LeaveRoomHandler(IServiceProvider services)
        {
            _roomManager = services.GetService<RoomManager>();
            _userDatabase = services.GetService<UserDatabase>();
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            socket.User.rooms.Remove(command.Room);
            _userDatabase.Save(socket.User.Name, socket.User);
            var room = _roomManager.GetRoom(command.Room);
            room.UnSubscribe(socket);
            room.Broadcast(new ChatMessage
            {
                Room = command.Room,
                Message = socket.User.Name + " has left the room",
                Sender = "[System]"
            });
        }
    }
}
