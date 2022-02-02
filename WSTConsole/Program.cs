using System;
using System.Net;
using WebSocketiny;
using WebSocketiny.Datatypes;

namespace WSTConsole
{
	static class Program
	{
		private static TcpServer tcpServer;
		static void Main()
		{
			TcpConfig tcpConfig = new()
			{
				ipAddress = IPAddress.Any,
				port = 6944,
			};

			tcpServer = new(tcpConfig);

			tcpServer.ReceivedMessage += ReceivedMessageHandler;

			tcpServer.Init();
		}

		private static void ReceivedMessageHandler(string message, int user)
		{
			Console.WriteLine("Message:");
			Console.WriteLine($"id {user} | {message}");

			tcpServer.Send(message, user);
		}

		private static void ConnectHandler(Client connectedClient)
		{
			Console.WriteLine("Connect:");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void DisconnectHandler(Client connectedClient)
		{
			Console.WriteLine("Disconnect:");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void ErrorHandler(Exception exception)
		{
			Console.WriteLine("Error:");
			Console.WriteLine($"Exception {exception}");
		}
	}
}
