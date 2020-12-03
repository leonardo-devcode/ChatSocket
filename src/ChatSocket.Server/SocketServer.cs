using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatSocket.Server.Models;
using ChatSocket.Server.Application;

namespace ChatSocket.Server
{

    public class SocketServer
    {
        TcpListener _server = null;
        ServiceManager serviceManager = new ServiceManager();

        public SocketServer(string ip, int port)
        {
            var localAddr = IPAddress.Parse(ip);
            _server = new TcpListener(localAddr, port);
            _server.Start();

            Console.WriteLine("Ready for a connection...");
            StartListener();
        }

        private void StartListener()
        {
            try
            {
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));

                    t.Start(serviceManager.AddUser(new User { Client = client })); ;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                _server.Stop();
            }
        }

        private void HandleDevice(Object obj)
        {
            User user = (User)obj;
            var stream = user.Client.GetStream();

            string payload = null;
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    payload = Encoding.ASCII.GetString(bytes, 0, i);
                    serviceManager.handleMessage(user, payload);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                user.Client.Close();
            }
        }

        

    }
}
