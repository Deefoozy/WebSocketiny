﻿using System;
using System.Net;
using WebSocketiny;
using WebSocketiny.DataTypes;

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

			tcpServer = new TcpServer(tcpConfig);

			tcpServer.ReceivedMessage += ReceivedMessageHandler;
			tcpServer.ClientConnected += ConnectHandler;
			tcpServer.ClientDisconnect += DisconnectHandler;

			tcpServer.Init();
		}

		private static void ReceivedMessageHandler(string message, int user)
		{
			Console.Write("Message:    ");
			Console.WriteLine($"id {user} | {message}");

			tcpServer.Send(message, user);
		}

		private static void ConnectHandler(Client connectedClient)
		{
			if (!connectedClient.active) return;

			Console.Write("Connect:    ");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void DisconnectHandler(Client connectedClient)
		{
			Console.Write("Disconnect: ");
			Console.WriteLine($"id {connectedClient.id}");
		}

		private static void ErrorHandler(Exception exception)
		{
			Console.WriteLine("Error:");
			Console.WriteLine($"Exception {exception}");
		}
	}
}
