using System.Threading.Tasks;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class DeleteRoomHandler : MessageHandler<RoomMessage>
    {
        private readonly RoomManager _roomManager;
        private readonly UserDatabase _userDatabase;

        public DeleteRoomHandler(RoomManager roomManager, UserDatabase userDatabase)
        {
            _roomManager = roomManager;
            _userDatabase = userDatabase;
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;//JsonSerializer.DeserializeFromString<DeleteRoom>(json);
            _roomManager.DeleteRoom(command.Room, socket.User.Name);
            socket.User.rooms.Remove(command.Room);
            _userDatabase.Save(socket.User.Name, socket.User);
        }
    }
}
