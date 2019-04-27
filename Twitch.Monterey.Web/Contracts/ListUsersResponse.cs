using System.Collections.Generic;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.Contracts
{
    class ListUsersResponse : RoomMessage
    {
        public List<string> Users { get; set; }

        public ListUsersResponse()
        {
            Op = "user-list";
        }
    }
}
