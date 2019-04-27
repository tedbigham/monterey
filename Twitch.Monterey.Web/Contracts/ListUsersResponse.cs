using System.Collections.Generic;

namespace Twitch.Monterey.Web.Contracts
{
    class ListUsersResponse : Message
    {
        public List<string> Users { get; set; }
    }
}
