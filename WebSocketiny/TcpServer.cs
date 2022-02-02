using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebSocketiny.ConnectionHandlers;
using WebSocketiny.DataTypes;
using WebSocketiny.ResponseHandlers;
using System.Collections.Generic;

namespace WebSocketiny
{
	public class TcpServer
	{
		readonly IPEndPoint _endpoint;

		private bool _isAccepting = true;
		private bool _initiated;
		private TcpListener? _server;
		private int _connectionAmount;
		private readonly ClientConnection _clientConnection;
		private readonly Dictionary<int, Client> _activeClients = new Dictionary<int, Client>();

		/// <summary>
		/// Method that initiates the websocket server
		/// </summary>
		public TcpServer(TcpConfig config)
		{
			_endpoint = new IPEndPoint(config.ipAddress, config.port);

			// Set up the clientConnection connectionHandler to be able to accept clients on multiple threads
			_clientConnection = new ClientConnection(_activeClients);
		}

		public void Stop()
		{
			Console.WriteLine("Shutting down TcpServer");
			_isAccepting = false;
			_server?.Stop();
		}

		public void Init()
		{
			if (_initiated)
				return;
			else
				_initiated = true;

			Console.WriteLine("Setting up clientConnection handler");

			// Setup server and start listening for connections
			_server = new TcpListener(_endpoint);
			_server.Start();

			Console.WriteLine($"Listener set up and listening on {_endpoint}");
			Console.WriteLine("Waiting for clients");

			// Loop that keeps accepting users and creates clients within the clientConnection until the _isAccepting is set to false.
			while (_isAccepting)
			{
				TcpClient client;

				// Wait until a client connects
				try
				{
					client = _server.AcceptTcpClient();
				}
				catch (SocketException exception)
				{
					Console.WriteLine("Server disconnect when waiting for client. {0}", exception.Message);
					return;
				}

				Client temporaryClient = new Client(_connectionAmount, client);

				temporaryClient.ReceivedMessageCallback += ReceivedMessage;
				temporaryClient.ConnectionCallback += ClientConnected;
				temporaryClient.DisconnectCallback += ClientDisconnect;

				// Start a new thread with the client and start that thread
				Thread newThread = new Thread(() =>
				{
					_clientConnection.Accept(temporaryClient);
					ClientConnected?.Invoke(temporaryClient);
				});

				newThread.Start();

				_connectionAmount++;

				// Instantly close to keep flow simple
				// _isAccepting = false;
			}
		}

		public void Send(string message, int userId)
		{
			MessageSender.SendToSpecific(message, _activeClients, userId);
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
