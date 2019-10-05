using System;
using System.Security.Cryptography;
using System.Text;

namespace WebSocketTest.Responses
{
	static class Handshake
	{
		private const string CLIENT_KEY_REQUEST_HEADER = "Sec-WebSocket-Key: ";
		private const string SERVER_KEY = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		/// <summary>
		/// Generates handshake message for new connections
		/// </summary>
		/// <param name="clientRequest"></param>
		/// <returns></returns>
		public static string GenerateHandshake(string clientRequest)
		{
			// Get position of the websocket key, then retrieve that key and add the server key to that.
			var secWebSocketKeyPosition = clientRequest.IndexOf(CLIENT_KEY_REQUEST_HEADER) + CLIENT_KEY_REQUEST_HEADER.Length;
			var receivedKey = clientRequest.Substring(secWebSocketKeyPosition, 24);
			var responseKey = receivedKey + SERVER_KEY;

			// Define end of line
			const string eol = "\r\n";
			var responseKeyHash = Convert.ToBase64String(
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
