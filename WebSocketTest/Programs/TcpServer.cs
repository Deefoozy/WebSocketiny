using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WebSocketTest.ConnectionHandlers;
using WebSocketTest.Models.Clients;

namespace WebSocketTest.Programs
{
    internal static class TcpServer
	{
		private static readonly IPEndPoint Endpoint = new IPEndPoint(IPAddress.Any, 8079); // TODO: auto free port finder 

		private static bool _isAccepting = true; //Im' not sure that should work in this way?
		public static int ConnectionAmount;

		/// <summary>
		/// Method that initiates the websocket server
		/// </summary>
		internal static void InitTcpServer()
		{
			Console.WriteLine("Setting up clientConnection handler");
			// Set up the clientConnection connectionHandler to be able to accept clients on multiple threads
			var clientConnection = new ClientConnection();

			// Setup server and start listening for connections
			var server = new TcpListener(Endpoint);
			server.Start();

			Console.WriteLine($"Listener set up and listening on {Endpoint}");
			Console.WriteLine("Waiting for clients");

			// Loop that keeps accepting users and creates clients within the clientConnection until the _isAccepting is set to false.
			while (_isAccepting)
			{
				// Wait until a ClientTcp connects
				var client = server.AcceptTcpClient();

				var temporaryClient = new Client(ConnectionAmount, client);

				Console.WriteLine($"Client | {ConnectionAmount}");

				// Start a new thread with the ClientTcp and start that thread
				var newThread = new Thread(() =>
				{
					clientConnection.Accept(temporaryClient);
				});
				newThread.Start();

				ConnectionAmount++;

				// Instantly close to keep flow simple
				// _isAccepting = false;
			}
		}
	}
}
