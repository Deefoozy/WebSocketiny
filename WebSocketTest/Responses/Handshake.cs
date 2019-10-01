using System;
using System.Security.Cryptography;
using System.Text;

namespace WebSocketTest.Responses
{
	static class Handshake
	{
		/// <summary>
		/// Name of the header send by the connecting client. Containing the key sent by the connecting client. Used to form the full response key
		/// </summary>
		private const string CLIENT_KEY_REQUEST_HEADER = "Sec-WebSocket-Key: ";
		private const string SERVER_KEY = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		public static byte[] GenerateHandshake(string clientRequest)
		{
			int secWebSocketKeyPosition = clientRequest.IndexOf(CLIENT_KEY_REQUEST_HEADER) + CLIENT_KEY_REQUEST_HEADER.Length;
			string receivedKey = clientRequest.Substring(secWebSocketKeyPosition, 24);
			string responseKey = receivedKey + SERVER_KEY;

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
