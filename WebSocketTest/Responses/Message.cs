using System;
using System.Text;
using System.IO;

namespace WebSocketTest.Responses
{
	class Message
	{
		/// <summary>
		/// Generates message that can be sent to a ClientTcp
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static byte[] GenerateMessage(string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte messageLength;
                byte[] buffer;

                // opCode = content type
				// FirstByte bits: 1 0 0 0 0 0 0 1
				// FirstByte bit 1: continuation. 1 if last message
				// FirstByte bit 2/4: reserved
				// FirstByte bit 5/8: opCode
				const byte opCode = 129;
				memoryStream.WriteByte(opCode);

				// Get byte of the message that has to be sent
				var payload = Encoding.UTF8.GetBytes(message);

				// Check the payload length to determine size and what bytes should be used to determine length
				if (payload.Length < 126)
				{
					// Assign payload.length to messageLength
					messageLength = (byte)(payload.Length);

					memoryStream.WriteByte(messageLength);
				}
				else if (payload.Length <= 65535)
				{
					// Assign a byte value of 126 to messageLength and use the 2 following bytes to designate message length
					messageLength = (byte)126;

					memoryStream.WriteByte(messageLength);

					buffer = BitConverter.GetBytes(payload.Length);

					if (BitConverter.IsLittleEndian)
						Array.Reverse(buffer);

					memoryStream.Write(buffer, 2, 2);
				}
				else
				{
					// Assign a byte value of 127 to messageLength and use the 4 following bytes to designate message length
					messageLength = (byte)127;

					memoryStream.WriteByte(messageLength);

					buffer = BitConverter.GetBytes(payload.Length);

					if (BitConverter.IsLittleEndian)
						Array.Reverse(buffer);

					memoryStream.Write(buffer, 0, buffer.Length);
				}

				// Write payload
				memoryStream.Write(payload, 0, payload.Length);

				// Return array with message as array
				return memoryStream.ToArray();
			}
		}
	}
}
