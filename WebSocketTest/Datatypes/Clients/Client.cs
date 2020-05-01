using System.Net.Sockets;
using WebSocketTest.Programs;

namespace WebSocketTest.Datatypes
{
	class Client
	{
		public readonly int id;
		public readonly TcpClient client;
		public readonly MessageEventCallback ReceivedMessageCallback;

		public Client(int passedClientId, TcpClient passedClient, MessageEventCallback receivedMessageCallback)
		{
			id = passedClientId;
			client = passedClient;
			ReceivedMessageCallback = receivedMessageCallback;
		}

		public void ExecCallback(string message) {
			ReceivedMessageCallback(message, id);
		}
	}
}
