using System.Net.Sockets;

namespace WebSocketTest.Datatypes
{
	class Client
	{
		public readonly int id;
		public readonly TcpClient client;

		public Client(int passedClientId, TcpClient passedClient)
		{
			id = passedClientId;
			client = passedClient;
		}
	}
}
