using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebSocketiny.Tests
{
	public class ProtocolTestClient
	{
		private string _connectionUri;

		public ProtocolTestClient(string connectionUri)
		{
			_connectionUri = connectionUri;
		}

		public async Task<OpenSocketConnection> OpenConnection()
		{
			Uri parsedUri = new Uri(_connectionUri);

			TcpClient client = new TcpClient();
			await client.ConnectAsync(parsedUri.Host, parsedUri.Port);

			return new OpenSocketConnection(client);
		}
	}

	public class OpenSocketConnection
	{
		private TcpClient _client;

		public NetworkStream Stream { get => _client.GetStream(); }

		public OpenSocketConnection(TcpClient client)
		{
			_client = client;
		}

		public void Close()
		{
			_client.Close();
		}
	}
}
