using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebSocketTest.Decoders
{
	class MessageDecoder
	{
		/// <summary>
		/// Decodes received message and determines if it should close the connection or not
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static ReceivedMessage DecodeMessage(byte[] message)
		{
			// Determine the length of the message
			var secondByte = message[1];
			var dataLength = secondByte & 127;
			// Points to the index of the first masking byte in the message
			var indexFirstMask = 2;

			// Determine indexFirstMask by looking at the payload length. if the message is empty, create a closing receivedMessage
			if (dataLength == 126)
				indexFirstMask = 4;
			else if (dataLength == 127)
				indexFirstMask = 10;
			else if (dataLength == 0)
				return new ReceivedMessage("", true);

			// Get the 4 masking bytes
			var keys = message.Skip(indexFirstMask).Take(4);
			var indexFirstDataByte = indexFirstMask + 4;

			// Create decoded byte array to store the decoded message
			var decoded = new byte[message.Length - indexFirstDataByte];

			// Decode the message
			for (int encodedIndex = indexFirstDataByte, decodedIndex = 0; encodedIndex < (dataLength + indexFirstDataByte); encodedIndex++, decodedIndex++)
				decoded[decodedIndex] = (byte)(message[encodedIndex] ^ keys.ElementAt(decodedIndex % 4));

			// Return decoded message
			return new ReceivedMessage(Encoding.UTF8.GetString(decoded, 0, decoded.Length), false);
		}
	}

	class ReceivedMessage
	{
		public readonly string content;
		public readonly bool close;

		public ReceivedMessage(string receivedContent, bool closeConnection)
		{
			content = receivedContent;
			close = closeConnection;
		}
	}
}
