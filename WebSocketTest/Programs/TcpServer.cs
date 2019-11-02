using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebSocketTest.ConnectionHandlers;
using WebSocketTest.Datatypes;

namespace WebSocketTest.Programs
{
	static class TcpServer
	{
		private static readonly IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 8080);

		private static bool isAccepting = true;
		public static int connectionAmount;

		/// <summary>
		/// Method that initiates the websocket server
		/// </summary>
		internal static void InitTcpServer()
		{
			Console.WriteLine("Setting up clientConnection handler");
			// Set up the clientConnection connectionHandler to be able to accept clients on multiple threads
			ClientConnection clientConnection = new ClientConnection();

			// Setup server and start listening for connections
			TcpListener server = new TcpListener(endpoint);
			server.Start();

			Console.WriteLine($"Listener set up and listening on {endpoint}");
			Console.WriteLine("Waiting for clients");

			// Loop that keeps accepting users and creates clients within the clientConnection untill the _isAccepting is set to false.
			while (isAccepting)
			{
				// Wait untill a client connects
				TcpClient client = server.AcceptTcpClient();

				Client temporaryClient = new Client(connectionAmount, client);

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
	}
}
