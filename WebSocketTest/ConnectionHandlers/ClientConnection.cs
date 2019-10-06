using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;
using WebSocketTest.Models.Clients;
using WebSocketTest.Models.Games;
using WebSocketTest.ResponseHandlers;

namespace WebSocketTest.ConnectionHandlers
{
	class ClientConnection
	{
		private readonly Dictionary<int, Client> activeClients = new Dictionary<int, Client>();
		private readonly List<Game> activeGames = new List<Game>();
		private int gameId;

		public ClientConnection()
		{
			for (int i = 0; i < 5; i++)
				activeGames.Add(new Game(gameId++));
		}

		/// <summary>
		/// Accept a new ClientTcp, by handshaking and starting to wait for messages
		/// </summary>
		/// <param name="clientData"></param>
		public void Accept(Client clientData)
		{
			// Wait untill part of the initial connection message has been received
			while (clientData.ClientTcp.Available < Encoding.UTF8.GetByteCount("GET"))
				Thread.Sleep(1);

			string data = Encoding.UTF8.GetString(ReadStream(clientData, true));

			// Check if http request
			if (data.StartsWith("GET"))
			{
				try
				{
					MessageSender.SendToSpecific(Handshake.GenerateHandshake(data), clientData);
				}
				catch (Exception exep)
				{
					Console.WriteLine($"{clientData.Id} | Could not accept ClientTcp connection | {exep}");
				}
			}
			else
			{
				Console.WriteLine($"{clientData.Id} | Could not accept ClientTcp connection");
				return;
			}

			// Add ClientTcp to active clients and assign that ClientTcp to a game
			activeClients.Add(clientData.Id, clientData);
			AssignToGame(clientData, activeGames);

			// Start waiting for messages, does not return untill ClientTcp disconnects
			WaitForMessage(clientData);

			// End connection with ClientTcp
			clientData.ClientTcp.GetStream().Close();
			Console.WriteLine($"{clientData.Id} | Closed ClientTcp connection");
		}

		/// <summary>
		/// Starts waiting for message, returns void when ClientTcp disconnects or connection has to be closed
		/// </summary>
		/// <param name="clientData"></param>
		private void WaitForMessage(Client clientData)
		{
			bool open = true;

			while (open)
			{
				// Close connection if the ClientTcp disconnected
				if (!clientData.ClientTcp.Connected)
					return;

				// Wait untill message data is available
				while (!clientData.ClientTcp.GetStream().DataAvailable)
				{
					// Check if ClientTcp is still connected
					if (clientData.ClientTcp.Client.Poll(10, SelectMode.SelectRead))
					{
						// Remove the ClientTcp and end WaitForMessage
						RemoveClient(clientData.Id);
						return;
					}

					// Put thread in low priority pool if no data is available
					Thread.Sleep(1);
				}

				ReceivedMessage incomingMessage = MessageDecoder.DecodeMessage(ReadStream(clientData));

				if (incomingMessage.close)
				{
					RemoveClient(clientData.Id);
					break;
				}

				MessageSender.SendToAll("a", activeClients.Values.ToList());

				// DO WHATEVS

				Console.WriteLine($"{clientData.Id} | {incomingMessage.content}");

				// byte[] resp = Message.GenerateMessage("                     Hello World                     Hello World                          Hello World                     Hello World                   a");
				// byte[] resp = Message.GenerateMessage("yes my dude");

				// stream.Write(resp, 0, resp.Length);

				// This is just here for testing connection closing
				// messageAmount++;
				// open = messageAmount < 50;
			}
		}

		/// <summary>
		/// Reads stream of specified user and returns the message bytes
		/// </summary>
		/// <param name="clientData"></param>
		/// <param name="initial"></param>
		/// <returns></returns>
		private byte[] ReadStream(Client clientData, bool initial = false)
		{
			byte[] bytes = new byte[initial ? clientData.ClientTcp.Available : 1024];
			clientData.ClientTcp.GetStream().Read(bytes, 0, bytes.Length);
			return bytes;
		}

		/// <summary>
		/// Removes ClientTcp from activeClients by id
		/// </summary>
		/// <param name="id"></param>
		private void RemoveClient(int id)
		{
			activeClients.Remove(id);
		}

		/// <summary>
		/// Assigns specified ClientTcp to a Game in the gamePool
		/// </summary>
		/// <param name="passedClient"></param>
		/// <param name="gamePool"></param>
		private void AssignToGame(Client passedClient, List<Game> gamePool)
		{
			for (int i = 0; i < gamePool.Count; i++)
			{
				if (gamePool[i].PlayerAmount < 2)
				{
					gamePool[i].AddPlayer(passedClient);
					break;
				}
			}
		}

		/// <summary>
		/// Updates gamelist
		/// </summary>
		private void UpdateGameList()
		{
			// Faulty logic | fix later
			if (activeGames.Count == 0)
			{
				activeGames.Add(new Game(gameId++));
			}
			else
			{
				for (int i = 0; i < activeGames.Count; i++)
				{
					if (activeGames[i].PlayerAmount == 0 && activeGames.Count > 2)
					{
						activeGames.RemoveAt(i);
						i--;
					}
					else if (activeGames[i].PlayerAmount == 2)
					{
						activeGames.Add(new Game(gameId++));
					}
				}
			}
		}
	}
}
