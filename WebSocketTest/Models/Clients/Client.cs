using System.Net.Sockets;

namespace WebSocketTest.Models.Clients
{
    internal class Client
	{
		public readonly int Id;
		public readonly TcpClient ClientTcp;

		public Client(int passedClientId, TcpClient passedClientTcp)
		{
			Id = passedClientId;
			ClientTcp = passedClientTcp;
		}
	}
}
