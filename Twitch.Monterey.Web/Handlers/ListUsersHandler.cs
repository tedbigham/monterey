using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitch.Monterey.Web.Managers;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class ListUsersHandler : MessageHandler<RoomMessage>
    {
        private RoomManager _roomManager;

        public ListUsersHandler(IServiceProvider services)
        {
            _roomManager = services.GetService<RoomManager>();
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var command = (RoomMessage) message;
            var users = _roomManager.GetRoom(command.Room).GetUsers();
            await socket.SendMessageAsync(new ListUsersResponse {Users = users, Room = command.Room});
        }
    }
}
