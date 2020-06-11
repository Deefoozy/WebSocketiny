using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebSocketiny.ConnectionHandlers;
using WebSocketiny.Datatypes;

namespace WebSocketiny
{
	public class TcpServer
	{
		readonly IPEndPoint _endpoint;

		private bool _isAccepting = true;
		private bool _initiated = false;
		private TcpListener _server;
		public int connectionAmount;

		/// <summary>
		/// Method that initiates the websocket server
		/// </summary>
		public TcpServer(TcpConfig config) {
			_endpoint = new IPEndPoint(config.ipAddress, config.port);
		}

		public void Stop()
		{
			_isAccepting = false;
			_server.Stop();
		}

		public void Init()
		{
			if (_initiated)
				return;
			else
				_initiated = true;

			Console.WriteLine("Setting up clientConnection handler");
			// Set up the clientConnection connectionHandler to be able to accept clients on multiple threads
			ClientConnection clientConnection = new ClientConnection();

			// Setup server and start listening for connections
			_server = new TcpListener(_endpoint);
			_server.Start();

			Console.WriteLine($"Listener set up and listening on {_endpoint}");
			Console.WriteLine("Waiting for clients");

			// Loop that keeps accepting users and creates clients within the clientConnection untill the _isAccepting is set to false.
			while (_isAccepting)
			{
				// Wait untill a client connects
				TcpClient client = _server.AcceptTcpClient();

				Client temporaryClient = new Client(connectionAmount, client);

				temporaryClient.ReceivedMessageCallback += ReceivedMessage;
				temporaryClient.DisconnectCallback += ClientDisconnect;

				Console.WriteLine($"Client | {connectionAmount}");

				// Start a new thread with the client and start that thread
				Thread newThread = new Thread(() =>
				{
					clientConnection.Accept(temporaryClient);
					ClientConnected?.Invoke(temporaryClient);
				});

				newThread.Start();

				connectionAmount++;

				// Instantly close to keep flow simple
				// _isAccepting = false;
			}
		}

		public event MessageEventCallback? ReceivedMessage;
		public event ConnectionEventCallback? ClientConnected;
		public event DisconnectEventCallback? ClientDisconnect;
		public event ErrorEventCallback? CaughtError;
	}

	public delegate void MessageEventCallback(string message, int user);
	public delegate void ConnectionEventCallback(Client connectedClient);
	public delegate void DisconnectEventCallback(Client connectedClient);
	public delegate void ErrorEventCallback(Exception exception);
}
