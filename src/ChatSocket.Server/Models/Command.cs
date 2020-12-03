using System;
using System.Collections.Generic;

namespace ChatSocket.Server.Models
{
    public class Command
    {
        public string Name { get; set; }

        public string Help { get; set; }

        public Func<string, User, List<User>, List<Room>, CommandResult> Handler { get; set; }
    }
}
