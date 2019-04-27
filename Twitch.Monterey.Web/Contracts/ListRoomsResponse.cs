using System.Collections.Generic;

namespace Twitch.Monterey.Web.Contracts
{
    class ListRoomsResponse : Message
    {
        public List<string> Rooms { get; set; }

        public ListRoomsResponse()
        {
            Op = "room-list";
        }
    }
}
