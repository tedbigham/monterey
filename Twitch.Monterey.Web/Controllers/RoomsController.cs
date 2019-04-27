using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Models;

namespace Twitch.Monterey.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        RoomDatabase db;
        public RoomsController(RoomDatabase db)
        {
            this.db = db;
        }

        // list all rooms
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return db.Keys();
        }

        // get a specific room
        [HttpGet("{id}")]
        public ActionResult<Room> Get(string id)
        {
            return db.Load(id);
        }

        // make a new room
        [HttpPost]
        public ActionResult<Room> Post([FromBody] Room room)
        {
            var existing = db.Load(room.Name);
            if (existing != null)
            {
                return BadRequest("exists");
            }
            room.Owner = "TBD";
            db.Save(room.Name, room);
            return room;
        }

        // remove a room and notifys connected users
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            db.Delete(id);
            // TODO notify
        }
    }
}
