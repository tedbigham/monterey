using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserDatabase db;

        public UsersController(UserDatabase db)
        {
            this.db = db;
        }

        // create a user
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("required: Name");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("required: Password");
            }
            var existing = db.Load(user.Name);
            if (existing != null)
            {
                return BadRequest("exists: " + user.Name);
            }
            user.rooms = new List<string>();
            db.Save(user.Name, user);
            user.Password = "...";
            return user;
        }
    }
}
