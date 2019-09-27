using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using WebSocketTest.ConnectionHandlers;
using WebSocketTest.Datatypes;

namespace WebSocketTest.Programs
{
	static class TcpServer
	{
		static private readonly IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 8080);

		static private bool _isAccepting = true;
		static public int connectionAmount = 0;

		// Maybe create a class with a thread that will contain a list of streams it could handle
		// Would make it easier to read probably
		// private List<Thread> currentThreads = new List<Thread>();

		internal static void InitTcpServer()
		{
			// Link to localhost, not to the outside world
			Console.WriteLine("Setting up clientConnection handler");
			ClientConnection clientConnection = new ClientConnection();

			TcpListener server = new TcpListener(endpoint);

			server.Start();

			Console.WriteLine($"Listener set up and listening on {endpoint}");
			Console.WriteLine("Waiting for clients");

			while (_isAccepting)
			{
				TcpClient client = server.AcceptTcpClient();

				Client temporaryClient = new Client(connectionAmount, client);

				Console.WriteLine($"Client | {connectionAmount}");

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
