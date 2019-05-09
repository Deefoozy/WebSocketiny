using System;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace WebSocketTest.Responses
{
    static class Handshake
    {
        public static byte[] GenerateHandshake(string data)
        {
            string secWebSocketString = "Sec-WebSocket-Key: ";
            int secWebSocketKeyPosition = data.IndexOf(secWebSocketString) + secWebSocketString.Count();
            string receivedKey = data.Substring(secWebSocketKeyPosition, 24);
            string responseKey = receivedKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

            const string eol = "\r\n";
            return Encoding.UTF8.GetBytes(
                "HTTP/1.1 101 Switching Protocols" + eol
                + "Connection: Upgrade" + eol
                + "Upgrade: websocket" + eol
                + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                    SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(responseKey)
                    )
                ) + eol
                + eol
            );
        }
    }
}
