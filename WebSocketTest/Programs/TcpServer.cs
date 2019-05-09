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
        private bool _isRunning = true;

        // Maybe create a class with a thread that will contain a list of streams it could handle
        // Would make it easier to read probably
        private List<Thread> currentThreads = new List<Thread>();

        public TcpServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            server.Start();

            Console.WriteLine("Listener set up and listening on 127.0.0.1:8080");
            while (_isRunning)
            {
                Console.WriteLine("Waiting for client");

                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine("Client accepted");

                ClientConnection clientConnection = new ClientConnection();

                Thread newThread = new Thread(new ParameterizedThreadStart(clientConnection.Accept));
                newThread.Start(client);
                currentThreads.Add(newThread);

                // Instantly close to keep flow simple
                _isRunning = false;
            }
            Console.ReadKey();
        }
    }
}
