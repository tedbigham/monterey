using System.Collections.Generic;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Managers;

namespace Twitch.Monterey.Web.Contracts
{
    // finds the right handler for the op code
    public class Mapper
    {
        private Dictionary<string, MessageHandler> handlers = new Dictionary<string, MessageHandler>();

        public Mapper(UserDatabase userDatabase, RoomDatabase roomDatabase, RoomManager roomManager)
        {
            handlers["auth"] = new AuthHandler(userDatabase, roomManager);
            handlers["message"] = new ChatMessageHandler(roomManager);
            handlers["create-room"] = new CreateRoomHandler(roomManager);
            handlers["delete-room"] = new DeleteRoomHandler(roomManager, userDatabase);
            handlers["join-room"] = new JoinRoomHandler(userDatabase, roomManager);
            handlers["leave-room"] = new LeaveRoomHandler(userDatabase, roomManager);
            handlers["list-rooms"] = new ListRoomsHandler(roomDatabase);
            handlers["list-users"] = new ListUsersHandler(roomManager);
        }

        public MessageHandler GetHandler(string op)
        {
            return handlers[op];
        }
    }
}
