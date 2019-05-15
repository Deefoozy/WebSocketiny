using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebSocketTest.Decoders
{
    class MessageDecoder
    {
        public static Message DecodeMessage(byte[] message)
        {
            byte secondByte = message[1];
            int dataLength = secondByte & 127;
            int indexFirstMask = 2;

            if (dataLength == 126)
                indexFirstMask = 4;
            else if (dataLength == 127)
                indexFirstMask = 10;
            else if (dataLength == 0)
                return new Message("", true);

            IEnumerable<byte> keys = message.Skip(indexFirstMask).Take(4);
            int indexFirstDataByte = indexFirstMask + 4;

            byte[] decoded = new byte[message.Length - indexFirstDataByte];
            for (int encodedIndex = indexFirstDataByte, decodedIndex = 0; encodedIndex < (dataLength + indexFirstDataByte); encodedIndex++, decodedIndex++)
            {
                decoded[decodedIndex] = (byte)(message[encodedIndex] ^ keys.ElementAt(decodedIndex % 4));
            }

            return new Message(Encoding.UTF8.GetString(decoded, 0, decoded.Length), false);
        }
    }

    class ReceivedMessage
    {
        public readonly string content;
        public readonly bool close;

        public Message(string receivedContent, bool closeConnection)
        {
            content = receivedContent;
            close = closeConnection;
        }
    }
}
