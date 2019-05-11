using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;

namespace WebSocketTest.ConnectionHandlers
{
    class ClientConnection
    {
        TcpClient client;
        NetworkStream stream;
        int messageAmount = 0;

        public ClientConnection(TcpClient tcpClient)
        {
            client = tcpClient;
            stream = client.GetStream();

            Accept();
        }

        public void Accept()
        {
            // TODO: Check if we need to wait for incoming data
            while (client.Available < Encoding.UTF8.GetByteCount("GET"))
            {
                // Wait for enough bytes to be available
            }
            
            string data = Encoding.UTF8.GetString(ReadStream(true));

            if (data.StartsWith("GET"))
            {
                SendHandshake(Handshake.GenerateHandshake(data));
            }

            WaitForMessage(data);

            stream.Close();
        }

        private void WaitForMessage(string data)
        {
            bool open = true;

            // Replace these loops for some async option for improved performance
            while (open)
            {
                // Sleeping for lower cpu usage
                while (!stream.DataAvailable)
                    Thread.Sleep(1);

                string incomingMessage = MessageDecoder.DecodeMessage(ReadStream());

                Console.WriteLine(incomingMessage);

                byte[] resp = Message.GenerateMessage("Hello World");

                stream.Write(resp, 0, resp.Length);

                // This is just here for testing connection closing
                messageAmount++;
                open = messageAmount < 5;
            }
            Console.WriteLine("Closed client connection");
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
