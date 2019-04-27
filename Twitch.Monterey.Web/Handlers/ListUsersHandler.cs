using System.Threading.Tasks;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class ListUsersHandler : MessageHandler<RoomMessage>
    {
        private RoomManager _roomManager;

        public ListUsersHandler(RoomManager roomManager) {
            _roomManager = roomManager;
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            var users = _roomManager.GetRoom(command.Room).GetUsers();
            await socket.SendMessageAsync(new ListUsersResponse {Users = users});
        }
    }
}
