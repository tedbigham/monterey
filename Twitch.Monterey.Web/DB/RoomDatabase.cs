using ServiceStack.Redis;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.DB
{
    public class RoomDatabase : Database<Room>
    {
        public RoomDatabase(IRedisClientsManager redisManager) : base(redisManager, "room")
        {
        }
    }
}
