using System;
using System.Collections.Generic;
using System.Text;

namespace ChatSocket.Server.Models
{
    public class CommandResult
    {
        public bool Closed { get; set; }

        public bool CreatedRoom { get; set; }

        public bool ChangedRoom { get; set; }

        public string RoomName { get; set; }
    }
}
