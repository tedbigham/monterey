using System;
using System.Collections.Generic;

namespace Twitch.Monterey.Web.Contracts
{
    // finds the right handler for the op code
    public class Mapper
    {
        private Dictionary<string, MessageHandler> handlers = new Dictionary<string, MessageHandler>();

        public Mapper(IServiceProvider services)
        {
            handlers["auth"] = new AuthHandler(services);
            handlers["message"] = new ChatMessageHandler(services);
            handlers["create-room"] = new CreateRoomHandler(services);
            handlers["delete-room"] = new DeleteRoomHandler(services);
            handlers["join-room"] = new JoinRoomHandler(services);
            handlers["leave-room"] = new LeaveRoomHandler(services);
            handlers["list-rooms"] = new ListRoomsHandler(services);
            handlers["list-users"] = new ListUsersHandler(services);
        }

        public MessageHandler GetHandler(string op)
        {
            return handlers[op];
        }
    }
}
