using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using WebSocketTest.ConnectionHandlers;

namespace WebSocketTest.Programs
{
    class TcpServer
    {
        private int _connectionAmount = 0;
        private bool _isAccepting = true;
        private readonly IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, 8080);

        // Maybe create a class with a thread that will contain a list of streams it could handle
        // Would make it easier to read probably
        private List<Thread> currentThreads = new List<Thread>();

        public TcpServer()
        {
            // Link to localhost, not to the outside world
            TcpListener server = new TcpListener(endpoint);

            server.Start();

            Console.WriteLine($"Listener set up and listening on {endpoint}");
            while (_isAccepting)
            {
                Console.WriteLine("Waiting for client");

                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine("Client accepted");

                Thread newThread = new Thread(() => {
                    new ClientConnection(client, _connectionAmount);
                });
                newThread.Start();
                // currentThreads.Add(newThread);

                // Instantly close to keep flow simple
                // _isAccepting = false;
            }
        }
    }
}
