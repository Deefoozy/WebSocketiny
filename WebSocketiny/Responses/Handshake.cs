using System;
using System.Security.Cryptography;
using System.Text;

namespace WebSocketiny.Responses
{
	static class Handshake
	{
		private const string clientKeyRequestHeader = "Sec-WebSocket-Key: ";
		private const string serverKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		/// <summary>
		/// Generates handshake message for new connections
		/// </summary>
		/// <param name="clientRequest"></param>
		/// <returns></returns>
		public static string GenerateHandshake(string clientRequest)
		{
			// Get position of the websocket key, then retrieve that key and add the server key to that.
			int secWebSocketKeyPosition = clientRequest.IndexOf(clientKeyRequestHeader, StringComparison.Ordinal) + clientKeyRequestHeader.Length;
			string receivedKey = clientRequest.Substring(secWebSocketKeyPosition, 24);
			string responseKey = receivedKey + serverKey;

			// Define end of line
			const string eol = "\r\n";
			string responseKeyHash = Convert.ToBase64String(
				SHA1.Create().ComputeHash(
					Encoding.UTF8.GetBytes(responseKey)
				)
			);

			return $"HTTP/1.1 101 Switching Protocols{eol}"
			       + $"Connection: Upgrade{eol}"
			       + $"Upgrade: websocket{eol}"
			       + $"Sec-WebSocket-Accept: {responseKeyHash}{eol}"
			       + eol;
		}
	}
}
