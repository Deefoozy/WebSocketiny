using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using WebSocketiny.Decoders;
using WebSocketiny.Responses;
using WebSocketiny.DataTypes;
using WebSocketiny.ResponseHandlers;

namespace WebSocketiny.ConnectionHandlers
{
	class ClientConnection
	{
		private readonly Dictionary<int, Client> _activeClients;

		public ClientConnection(Dictionary<int, Client> activeClients)
		{
			_activeClients = activeClients;
		}

		/// <summary>
		/// Accept a new client, by handshaking and starting to wait for messages
		/// </summary>
		/// <param name="clientData"></param>
		public void Accept(Client clientData)
		{
			// Wait until part of the initial connection message has been received
			while (clientData.client.Available < Encoding.UTF8.GetByteCount("GET"))
				Thread.Sleep(1);

			string data = Encoding.UTF8.GetString(ReadStream(clientData, true));

			// Check if http request
			if (data.StartsWith("GET"))
			{
				try
				{
					MessageSender.SendToSpecific(Handshake.GenerateHandshake(data), clientData);
				}
				catch (Exception exception)
				{
					Console.WriteLine($"{clientData.id} | Could not accept client connection | {exception}");
				}
			}
			else
			{
				Console.WriteLine($"{clientData.id} | Could not accept client connection");
				return;
			}

			// Add client to active clients and assign that client to a game
			_activeClients.Add(clientData.id, clientData);

			// Start waiting for messages, does not return until client disconnects
			WaitForMessage(clientData);

			// End connection with client
			clientData.stream.Close();
			Console.WriteLine($"{clientData.id} | Closed client connection");
		}

		/// <summary>
		/// Starts waiting for message, returns void when client disconnects or connection has to be closed
		/// </summary>
		/// <param name="clientData"></param>
		private void WaitForMessage(Client clientData)
		{
			while (true)
			{
				// Close connection if the client disconnected
				if (!clientData.client.Connected)
					break;

				// Wait until message data is available
				while (!clientData.stream.DataAvailable)
				{
					// TODO Check if client is still connected

					// Put thread in low priority pool if no data is available
					Thread.Sleep(1);
				}

				ReceivedMessage incomingMessage = MessageDecoder.DecodeMessage(ReadStream(clientData));

				if (incomingMessage.close)
				{
					RemoveClient(clientData.id);
					break;
				}

				clientData.ExecMessageCallback(incomingMessage.content);
			}
		}

		/// <summary>
		/// Reads stream of specified user and returns the message bytes
		/// </summary>
		/// <param name="clientData"></param>
		/// <param name="initial"></param>
		/// <returns>Read bytes</returns>
		private static byte[] ReadStream(Client clientData, bool initial = false)
		{
			byte[] bytes = new byte[initial ? clientData.client.Available : 1024];
			clientData.stream.Read(bytes, 0, bytes.Length);
			return bytes;
		}

		/// <summary>
		/// Removes client from activeClients by id
		/// </summary>
		/// <param name="id"></param>
		private void RemoveClient(int id)
		{
			_activeClients.Remove(id);
		}
	}
}
