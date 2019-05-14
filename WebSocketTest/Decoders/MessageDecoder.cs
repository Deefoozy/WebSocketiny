﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebSocketTest.Decoders
{
    class MessageDecoder
    {
        public static string DecodeMessage(byte[] message)
        {
            byte secondByte = message[1];
            int dataLength = secondByte & 127;
            int indexFirstMask = 2;

            if (dataLength == 126)
                indexFirstMask = 4;
            else if (dataLength == 127)
                indexFirstMask = 10;

            IEnumerable<byte> keys = message.Skip(indexFirstMask).Take(4);
            int indexFirstDataByte = indexFirstMask + 4;

            byte[] decoded = new byte[message.Length - indexFirstDataByte];
            for (int encodedIndex = indexFirstDataByte, decodedIndex = 0; encodedIndex < (dataLength + indexFirstDataByte); encodedIndex++, decodedIndex++)
            {
                // Console.WriteLine(buffer[encodedIndex] + " | " + keys.ElementAt(decodedIndex % 4) + " | " + (byte)(buffer[encodedIndex] ^ keys.ElementAt(decodedIndex % 4)));
                decoded[decodedIndex] = (byte)(message[encodedIndex] ^ keys.ElementAt(decodedIndex % 4));
            }

            return Encoding.UTF8.GetString(decoded, 0, decoded.Length); ;
        }
    }
}