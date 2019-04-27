using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace Twitch.Monterey.Web.DB
{
    /// <summary>
    /// Naive storage implementation
    /// This uses a redis hash
    /// </summary>
    public class Database<T>
    {
        protected IRedisClient redisClient;

        protected string key;

        public Database(IRedisClientsManager redisManager, string key)
        {
            this.key = key;
            redisClient = redisManager.GetClient();
        }

        public List<string> Keys()
        {
            return redisClient.Hashes[key].Keys.ToList();
        }

        public T Load(string id)
        {
            var json = redisClient.Hashes[key][id];
            if (json == null)
            {
                return default(T);
            }

            return JsonSerializer.DeserializeFromString<T>(json);
        }

        public void Save(string id, T value)
        {
            var json = JsonSerializer.SerializeToString(value);
            redisClient.Hashes[key][id] = json;
        }

        public void Delete(string id)
        {
            redisClient.Hashes[key].Remove(id);
        }
    }
}
