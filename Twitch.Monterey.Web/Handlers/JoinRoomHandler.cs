using System;
using System.Threading.Tasks;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class JoinRoomHandler : MessageHandler<RoomMessage>
    {
        private readonly RoomManager _roomManager;
        private readonly UserDatabase _userDatabase;

        public JoinRoomHandler(UserDatabase userDatabase, RoomManager roomManager)
        {
            _roomManager = roomManager;
            _userDatabase = userDatabase;
    }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            if (string.IsNullOrEmpty(command.Room))
            {
                throw new Exception("missing: room");
            }

            if (!socket.User.rooms.Contains(command.Room))
            {
                socket.User.rooms.Add(command.Room);
                _userDatabase.Save(socket.User.Name, socket.User);
            }
            var room = _roomManager.GetRoom(command.Room);
            room.Subscribe(socket);
            room.Broadcast(new ChatMessage
            {
                Room=command.Room,
                Message = socket.User.Name + " has joined the room",
                Sender ="[System]"
            });
        }
    }
}
