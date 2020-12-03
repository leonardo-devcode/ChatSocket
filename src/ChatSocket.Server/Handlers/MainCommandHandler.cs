using System.Collections.Generic;
using System.Linq;
using ChatSocket.Server.Models;

namespace ChatSocket.Server.Handlers
{
    public abstract class MainCommandHandler
    {
        public User CurrentUser { get; set; }
        public List<Room> Rooms { get; set; }
        public List<User> Users { get; set; }

        protected void SendMessageSocketCurrentUser(string message)
        {
            CurrentUser.SendMessage(message);
        }

        protected void SendMessageSocketUser(string message, User user)
        {
            user.SendMessage(message);
        }

        protected void SendMessageSocketRoom(string message)
        {
            Users
                .Where(p => !p.Nickname.Equals(CurrentUser.Nickname))
                .Where(p => p.Room.Name.Equals(CurrentUser.Room.Name))
                .ToList()
                .ForEach(user =>
                {
                    user.SendMessage(message);
                });
        }
    }
}
