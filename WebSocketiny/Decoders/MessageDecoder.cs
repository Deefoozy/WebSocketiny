using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebSocketiny.Decoders
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
			byte secondByte = message[1];
			int dataLength = secondByte & 127;
			// Points to the index of the first masking byte in the message
			int indexFirstMask = 2;

			// Determine indexFirstMask by looking at the payload length.
			switch (dataLength)
			{
				case 126:
					indexFirstMask = 4;
					break;
				case 127:
					indexFirstMask = 10;
					break;
				case 0:
					return new ReceivedMessage("", true);
			}

			// Get the 4 masking bytes
			IEnumerable<byte> keys = message.Skip(indexFirstMask).Take(4);
			int indexFirstDataByte = indexFirstMask + 4;

			// Create decoded byte array to store the decoded message
			byte[] decoded = new byte[dataLength];

			// Decode the message
			for (int encodedIndex = indexFirstDataByte, decodedIndex = 0; encodedIndex < dataLength + indexFirstDataByte; encodedIndex++, decodedIndex++)
				decoded[decodedIndex] = (byte)(message[encodedIndex] ^ keys.ElementAt(decodedIndex % 4));

			// Return decoded message
			return new ReceivedMessage(Encoding.UTF8.GetString(decoded, 0, decoded.Length), false);
		}
	}

	struct ReceivedMessage
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
