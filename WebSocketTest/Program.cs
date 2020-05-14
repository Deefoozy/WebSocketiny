using System;
using System.Net;
using WebSocketTest.Programs;

namespace WebSocketTest
{
	class Program
	{
		static void Main(string[] args)
		{
			TcpServer tcpServer = new TcpServer(new IPEndPoint(IPAddress.Any, 8080));
			tcpServer.ReceivedMessage += Log;
			tcpServer.Init();
		}

		static void Log(string message, int user) {
			Console.WriteLine($"{user} | {message}");
		}
	}
}
