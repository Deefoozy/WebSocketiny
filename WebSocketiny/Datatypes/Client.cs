using System.Net.Sockets;

namespace WebSocketiny.DataTypes
{
	public class Client
	{
		public readonly int id;
		public readonly TcpClient client;
		public readonly NetworkStream stream;
		public bool active = true;

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

		public void ExecConnectionCallback()
		{
			ConnectionCallback?.Invoke(this);
		}

		public void ExecDisconnectCallback()
		{
			DisconnectCallback?.Invoke(this);
		}

		public event MessageEventCallback? ReceivedMessageCallback;
		public event ConnectionEventCallback? ConnectionCallback;
		public event DisconnectEventCallback? DisconnectCallback;
	}
}
