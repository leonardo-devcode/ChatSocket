using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatSocket.Client
{
    class Program
    {
        // TODO: Mudar configurações para arquivo de configuração
        const string IP = "127.0.0.1";
        const int PORT = 13000;

        static void Main(string[] args)
        {
            Thread.CurrentThread.IsBackground = true;

            Console.WriteLine("Informe o seu apelido para começar o chat, depois vc podera troca-lo :)");

            var nickname = Console.ReadLine();

            Connect(nickname);

        }

        static void Connect(string nickname)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient(IP, PORT);
                stream = client.GetStream();

                SendCommandChangeNickname(stream, nickname);

                Byte[] payload = new Byte[256];

                Console.WriteLine($"Seja bem vindo {nickname}, se desejar receber os comandos disponiveis, envio /ajuda");
                Console.WriteLine($"Você esta na sala geral, usse o comando /salas para listas as salas disponiveis");

                // Executa tarefa que escutas as novas mensagens
                Task.Run(() => ListenMessages(stream));

                // Captura input do usuario
                string message = Console.ReadLine();
                while (true) {
                    payload = System.Text.Encoding.ASCII.GetBytes(message);
                    stream.Write(payload, 0, payload.Length);
                    message = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            } finally
            {
                stream?.Close();
                client?.Close();
            }
        }

        private static void SendCommandChangeNickname(NetworkStream stream, string nickname)
        {
            Byte[] payload = System.Text.Encoding.ASCII.GetBytes($"/apelido {nickname}");
            stream.Write(payload, 0, payload.Length);
        }

        private static void ListenMessages(NetworkStream stream)
        {
            Byte[] bytes = new Byte[256];
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                string hex = BitConverter.ToString(bytes);
                string response = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                Console.WriteLine(response);
            }

        }
    }
}
