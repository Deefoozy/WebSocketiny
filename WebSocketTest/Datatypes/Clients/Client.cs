using System.Net.Sockets;

namespace WebSocketTest.Datatypes
{
	class Client
	{
		public readonly TcpClient client;
		public readonly int id;

		public Client(int passedClientId, TcpClient passedClient)
		{
			id = passedClientId;
			client = passedClient;
		}
	}
}
