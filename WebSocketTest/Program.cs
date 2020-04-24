using System.Net;
using WebSocketTest.Programs;

namespace WebSocketTest
{
	class Program
	{
		static void Main(string[] args)
		{
			new TcpServer(new IPEndPoint(IPAddress.Any, 8080));
		}
	}
}
