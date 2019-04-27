using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.Script;
using Twitch.Monterey.Web.Contracts;
using Twitch.Monterey.Web.DB;
using Twitch.Monterey.Web.Models;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Managers
{
    /// <summary>
    /// Keeps track of all the active chat rooms
    /// </summary>
    public class RoomManager
    {
        private Dictionary<string, ChatRoom> rooms = new Dictionary<string, ChatRoom>();
        private RoomDatabase _roomDatabase;

        public RoomManager(RoomDatabase roomDatabase)
        {
            _roomDatabase = roomDatabase;
        }

        // this will start a new chat room if there is a saved room in the db
        public ChatRoom GetRoom(string name)
        {
            if (rooms.ContainsKey(name))
                return rooms[name];

            var saved = _roomDatabase.Load(name);
            if (saved == null)
            {
                throw new RoomNotFoundException("room not found: " + name);
            }
            var newRoom = new ChatRoom { Name = name };
            rooms[name] = newRoom;
            return newRoom;
        }

        public void DeleteRoom(string name, string owner)
        {
            // delete the room from the db
            var room = _roomDatabase.Load(name);

            if (room == null)  // already deleted???
            {
                if (rooms.ContainsKey(name))
                {
                    rooms.Remove(name);
                }
                return;
            }

            if (room.Owner != owner)
            {
                throw new Exception("not owner");
            }
            _roomDatabase.Delete(name);

            // unsubscribe everyone
            var chatRoom = GetRoom(name);
            chatRoom.Broadcast(new ChatMessage
            {
                Room = name,
                Message = "this room was deleted",
                Sender = "[System]"
            });
            rooms.Remove(name); // this probably leaked a bunch of stuff
        }

        public void CreateRoom(string name, string owner)
        {
            var existing = _roomDatabase.Load(name);
            if (existing != null)
            {
                throw new Exception("room already exists");
            }
            _roomDatabase.Save(name, new Room() {Name = name, Owner = owner});
        }

        public List<string> ListRooms()
        {
            return _roomDatabase.Keys();
        }
    }

    public class ChatRoom
    {
        public string Name;
        private event Action<ChatMessage> OnChatMessage;
        private event Action<List<string>> QueryUserName;

        public void Subscribe(ClientSocket socket)
        {
            UnSubscribe(socket);
            OnChatMessage += socket.OnChatMessage;
            QueryUserName += socket.OnQueryUserName;
        }

        public void UnSubscribe(ClientSocket socket)
        {
            OnChatMessage -= socket.OnChatMessage;
            QueryUserName -= socket.OnQueryUserName;
        }

        public void Broadcast(ChatMessage message)
        {
            OnChatMessage(message);
        }

        public List<string> GetUsers()
        {
            var result = new List<string>();
            QueryUserName(result);
            return result;
        }
    }

    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(string msg) : base(msg)
        {
        }
    }
}
