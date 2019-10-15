using System.Net.Sockets;

namespace WebSocketTest.Datatypes.Clients
{
	public class Client
	{
		public int Id { get; }
		public TcpClient TcpClient { get; }

		public Client(int passedClientId, TcpClient passedTcpClient)
		{
			Id = passedClientId;
			TcpClient = passedTcpClient;
		}
	}
}
