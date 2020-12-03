using System;
using System.Net.Sockets;
using System.Text;

namespace ChatSocket.Server.Models
{
    public class User
    {
        public string Nickname { get; set; }

        public TcpClient Client { get; set; }

        public Room Room { get; set; }

        public virtual void SendMessage(string message)
        {
            Byte[] reply = Encoding.ASCII.GetBytes(message);
            var stream = this.Client.GetStream();
            stream.Write(reply, 0, reply.Length);
        }

    }
}
