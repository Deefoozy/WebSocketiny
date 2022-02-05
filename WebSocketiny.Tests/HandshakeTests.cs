using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WebSocketiny.Tests
{
    public class HandshakeTests
    {
		private Thread _serverThread;

		public HandshakeTests()
		{

			_serverThread = new Thread(() => 
			{
				TcpServer server = new TcpServer(new DataTypes.TcpConfig()
				{
					ipAddress = IPAddress.Loopback,
					port = 4360
				});

				// Blocks
				server.Init();
			});
			// Make the project stoppable by marking the thread as a background thread
			// When the main thread ends all background threads are forcefully terminated
			_serverThread.IsBackground = true;
			_serverThread.Start();
		}

        [Fact]
        public async Task ClientCanConnectToServer()
        {
			ProtocolTestClient client = new ProtocolTestClient("ws://localhost:4360");

			OpenSocketConnection connection = await client.OpenConnection();

			connection.Close();
        }
	}
}
