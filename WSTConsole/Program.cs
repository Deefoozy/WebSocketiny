using System;
using System.Net;
using WebSocketiny;
using WebSocketiny.Datatypes;

namespace WSTConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var tcpConfig = new TcpConfig
			{
				ipAddress = IPAddress.Any,
				port = 6942,
			};

			var tcpServer = new TcpServer(tcpConfig);

			tcpServer.ReceivedMessage += ReceivedMessageHandler;

			tcpServer.Init();
		}

		private static void ReceivedMessageHandler(string message, int user) {
			Console.WriteLine($"Message:");
			Console.WriteLine($"id {user} | {message}");
		}

		private static void ConnectHandler(Client connectedClient)
		{
			Console.WriteLine($"Connect:");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void DisconnectHandler(Client connectedClient)
		{
			Console.WriteLine($"Disconnect:");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void ErrorHandler(Exception exception)
		{
			Console.WriteLine($"Error:");
			Console.WriteLine($"Exception {exception}");
		}
	}
}
