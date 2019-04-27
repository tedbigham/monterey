using System.Threading.Tasks;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class CreateRoomHandler : MessageHandler<RoomMessage>
    {
        private readonly RoomManager _roomManager;

        public CreateRoomHandler(RoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            _roomManager.CreateRoom(command.Room, socket.User.Name);
            //JoinRoom(command);
        }
    }
}
