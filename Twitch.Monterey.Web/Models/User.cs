using System.Collections.Generic;

namespace Twitch.Monterey.Web.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string> rooms { get; set; }
    }
}
