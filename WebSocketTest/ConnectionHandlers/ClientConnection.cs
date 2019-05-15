using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;

namespace WebSocketTest.ConnectionHandlers
{
    class ClientConnection
    {
        readonly TcpClient client;
        readonly NetworkStream stream;
        public readonly int id;
        int messageAmount = 0;
        

        public ClientConnection(TcpClient tcpClient, int clientId)
        {
            client = tcpClient;
            stream = client.GetStream();
            id = clientId;

            Accept();
        }

        public void Accept()
        {
            // TODO: Check if we need to wait for incoming data
            while (client.Available < Encoding.UTF8.GetByteCount("GET"))
                Thread.Sleep(1);
            
            string data = Encoding.UTF8.GetString(ReadStream(true));

            if (data.StartsWith("GET"))
            {
                SendHandshake(Handshake.GenerateHandshake(data));
            }

            WaitForMessage();

            Console.WriteLine($"{id} | Closed client connection");

            stream.Close();
        }

        private void WaitForMessage()
        {
            bool open = true;

            while (open)
            {
                // Putting thread into low priority
                while (!stream.DataAvailable)
                    Thread.Sleep(1);

                ReceivedMessage incomingMessage = MessageDecoder.DecodeMessage(ReadStream());

                if (incomingMessage.close)
                    break;

                Console.WriteLine($"{id} | {incomingMessage.content}");

                byte[] resp = Message.GenerateMessage("Hello World");

                stream.Write(resp, 0, resp.Length);

                // This is just here for testing connection closing
                // messageAmount++;
                // open = messageAmount < 50;
            }
        }

        private void SendHandshake(byte[] handshake)
        {
            stream.Write(handshake, 0, handshake.Length);
        }

        private byte[] ReadStream(bool initial = false)
        {
            byte[] bytes = new byte[initial ? client.Available : 1024];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
