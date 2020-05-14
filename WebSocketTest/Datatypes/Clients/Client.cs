using System.Net.Sockets;
using WebSocketTest.Programs;

namespace WebSocketTest.Datatypes
{
	public class Client
	{
		public readonly int id;
		public readonly TcpClient client;

		public Client(int passedClientId, TcpClient passedClient)
		{
			id = passedClientId;
			client = passedClient;
		}

		public void ExecMessageCallback(string message)
		{
			ReceivedMessageCallback(message, id);
		}

		public void ExecDisconnectCallback()
		{
			DisconnectCallback(this);
		}

		public event MessageEventCallback ReceivedMessageCallback;
		public event DisconnectEventCallback DisconnectCallback;
	}
}
