using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebSocketTest.ConnectionHandlers;
using WebSocketTest.Datatypes;

namespace WebSocketTest.Programs
{
	class TcpServer
	{
		readonly IPEndPoint _endpoint;

		private bool _isAccepting = true;
		private bool _initiated = false;
		public int connectionAmount;

		/// <summary>
		/// Method that initiates the websocket server
		/// </summary>
		public TcpServer(IPEndPoint endpoint)
		{
			_endpoint = endpoint;
		}

		public TcpServer(IPAddress ipAddress, int port)
		{
			_endpoint = new IPEndPoint(ipAddress, port);
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
			TcpListener server = new TcpListener(_endpoint);
			server.Start();

			Console.WriteLine($"Listener set up and listening on {_endpoint}");
			Console.WriteLine("Waiting for clients");

			// Loop that keeps accepting users and creates clients within the clientConnection untill the _isAccepting is set to false.
			while (_isAccepting)
			{
				// Wait untill a client connects
				TcpClient client = server.AcceptTcpClient();

				Client temporaryClient = new Client(connectionAmount, client, EReceivedMessage);

				Console.WriteLine($"Client | {connectionAmount}");

				// Start a new thread with the client and start that thread
				Thread newThread = new Thread(() =>
				{
					clientConnection.Accept(temporaryClient);
				});

				newThread.Start();

				connectionAmount++;

				// Instantly close to keep flow simple
				// _isAccepting = false;
			}
		}

		public event MessageEventCallback EReceivedMessage;
	}

	public delegate void MessageEventCallback(string message, int user);
}
