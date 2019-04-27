using ServiceStack.Redis;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.DB
{
    public class UserDatabase : Database<User>
    {
        public UserDatabase(IRedisClientsManager redisManager) : base(redisManager, "user")
        {
        }
    }
}
