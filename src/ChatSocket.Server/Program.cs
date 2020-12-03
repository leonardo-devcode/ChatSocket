using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatSocket.Server
{
    class Program
    {
        // TODO: Mudar configurações para arquivo de configuração
        const string IP = "127.0.0.1";
        const int PORT = 13000;

        static void Main(string[] args)
        {
            Thread t = new Thread(delegate ()
            {
                SocketServer myserver = new SocketServer(IP, PORT);
            });
            t.Start();

            Console.WriteLine("Server Started...!");
        }
    }
}
