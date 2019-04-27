using System;
using System.Threading.Tasks;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class AuthHandler : MessageHandler<AuthMessage>
    {
        private readonly UserDatabase _userDatabase;
        private readonly RoomManager _roomManager;

        public AuthHandler(UserDatabase userDatabase, RoomManager roomManager)
        {
            _userDatabase = userDatabase;
            _roomManager = roomManager;
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var auth = (AuthMessage) message;
            if (string.IsNullOrEmpty(auth.Name)) {
                throw new Exception("missing: name");
            }
            if (string.IsNullOrEmpty(auth.Password)) {
                throw new Exception("missing: password");
            }
            var user = _userDatabase.Load(auth.Name);
            if (user == null || user.Password != auth.Password)
            {
                throw new Exception("auth failed");
            }

            socket.User = user;
            foreach (var roomName in user.rooms.ToArray())
            {
                try
                {
                    var room = _roomManager.GetRoom(roomName);
                    room.Subscribe(socket);
                }
                catch (RoomNotFoundException e)
                {
                    user.rooms.Remove(roomName);
                    _userDatabase.Save(user.Name, user);
                }
            }
        }
    }
}
