using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WebSocketTest.Responses
{
	class Message
	{
		static public byte[] GenerateMessage(string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				// opCode = content type
				// FirstByte bits: 1 0 0 0 0 0 0 1
				// FirstByte bit 1: continuation. 1 if last message
				// FirstByte bit 2/4: reserved
				// FirstByte bit 5/8: opCode
				byte opCode = 129;
				memoryStream.WriteByte(opCode);

				// Console.WriteLine("10000001");
				// Console.WriteLine(Convert.ToString(opCode, 2).PadLeft(8, '0'));

				byte[] payload = Encoding.UTF8.GetBytes(message);

				byte messageLength;
				byte[] buffer;

				if (payload.Length < 126)
				{
					messageLength = (byte)(payload.Length);

					memoryStream.WriteByte(messageLength);
				}
				else if (payload.Length <= 65535)
				{
					messageLength = (byte)126;

					memoryStream.WriteByte(messageLength);

					buffer = BitConverter.GetBytes(payload.Length);

					if (BitConverter.IsLittleEndian)
						Array.Reverse(buffer);

					memoryStream.Write(buffer, 2, 2);
				}
				else
				{
					messageLength = (byte)127;

					memoryStream.WriteByte(messageLength);

					buffer = BitConverter.GetBytes(payload.Length);

					if (BitConverter.IsLittleEndian)
						Array.Reverse(buffer);

					memoryStream.Write(buffer, 0, buffer.Length);
				}

				memoryStream.Write(payload, 0, payload.Length);

				return memoryStream.ToArray();
			}
		}
	}
}
