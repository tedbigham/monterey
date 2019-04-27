using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public class ListRoomsHandler : MessageHandler<Message>
    {
        private readonly RoomDatabase _roomDatabase;

        public ListRoomsHandler(IServiceProvider services)
        {
            _roomDatabase = services.GetService<RoomDatabase>();
        }

        public override async Task HandleMessage(object message, ClientSocket socket)
        {
            var rooms = _roomDatabase.Keys();
            await socket.SendMessageAsync(new ListRoomsResponse { Rooms = rooms });
        }
    }
}
