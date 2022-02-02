using System.Net.Sockets;

namespace WebSocketiny.Datatypes
{
	public class Client
	{
		public readonly int id;
		public readonly TcpClient client;
		public readonly NetworkStream stream;

		public Client(int passedClientId, TcpClient passedClient)
		{
			id = passedClientId;
			client = passedClient;
			stream = passedClient.GetStream();
		}

		public void ExecMessageCallback(string message)
		{
			ReceivedMessageCallback?.Invoke(message, id);
		}

		public void ExecDisconnectCallback()
		{
			DisconnectCallback?.Invoke(this);
		}

		public event MessageEventCallback? ReceivedMessageCallback;
		public event DisconnectEventCallback? DisconnectCallback;
	}
}
