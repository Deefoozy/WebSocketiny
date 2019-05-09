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

        public void Accept(object tcpClient)
        {
            client = (TcpClient)tcpClient;
            stream = client.GetStream();

            while (client.Available < 3)
            {// wait for enough bytes to be available
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
                // sleeping for lower cpu usage
                while (!stream.DataAvailable) Thread.Sleep(500);

                byte[] resp = MessageDecoder.DecodeMessage(ReadStream());

                stream.Write(resp, 0, resp.Length);

                // This is just here for testing connection closing
                messageAmount++;
                open = messageAmount < 5;
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
