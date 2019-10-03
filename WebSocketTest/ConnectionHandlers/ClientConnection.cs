using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;
using WebSocketTest.Datatypes;
using WebSocketTest.ResponseHandlers;
using Newtonsoft.Json;

namespace WebSocketTest.ConnectionHandlers
{
	class ClientConnection
	{
		public List<Client> activeClients = new List<Client>();
		public List<Game> activeGames = new List<Game>();
		private int gameId = 0;

		public ClientConnection()
		{
			for (int i = 0; i < 5; i++)
				activeGames.Add(new Game(gameId++));
		}

		/// <summary>
		/// Accept a new client, by handshaking and starting to wait for messages
		/// </summary>
		/// <param name="clientData"></param>
		public void Accept(Client clientData)
		{
			// Wait untill part of the initial connection message has been received
			while (clientData.client.Available < Encoding.UTF8.GetByteCount("GET"))
				Thread.Sleep(1);

			string data = Encoding.UTF8.GetString(ReadStream(clientData, true));

			// Check if http request
			if (data.StartsWith("GET"))
			{
				try
				{
                    MessageSender.SendToSpecific(data, clientData);
				}
				catch (Exception exep)
				{
					Console.WriteLine($"{clientData.id} | Could not accept client connection | {exep}");
				}
			}
			else
			{
				Console.WriteLine($"{clientData.id} | Could not accept client connection");
				return;
			}

			// Add client to active clients and assign that client to a game
			activeClients.Add(clientData);
			AssignToGame(clientData, activeGames);

			// Start waiting for messages, does not return untill client disconnects
			WaitForMessage(clientData);

			// End connection with client
			clientData.client.GetStream().Close();
			Console.WriteLine($"{clientData.id} | Closed client connection");
		}

		/// <summary>
		/// Starts waiting for message, returns void when client disconnects or connection has to be closed
		/// </summary>
		/// <param name="clientData"></param>
		private void WaitForMessage(Client clientData)
		{
			bool open = true;

			while (open)
			{
				// Close connection if the client disconnected
				if (!clientData.client.Connected)
					return;

				// Wait untill message data is available
				while (!clientData.client.GetStream().DataAvailable)
				{
					// Check if client is still connected
					if (clientData.client.Client.Poll(10, SelectMode.SelectRead))
					{
						// Remove the client and end WaitForMessage
						RemoveClient(clientData.id);
						return;
					}

					// Put thread in low priority pool if no data is available
					Thread.Sleep(1);
				}

				ReceivedMessage incomingMessage = MessageDecoder.DecodeMessage(ReadStream(clientData));

				if (incomingMessage.close)
				{
					RemoveClient(clientData.id);
					break;
				}

				MessageSender.SendToAll("a", activeClients);

				// DO WHATEVS

				Console.WriteLine($"{clientData.id} | {incomingMessage.content}");

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
			byte[] bytes = new byte[initial ? clientData.client.Available : 1024];
			clientData.client.GetStream().Read(bytes, 0, bytes.Length);
			return bytes;
		}

		/// <summary>
		/// Removes client from activeClients by id
		/// </summary>
		/// <param name="id"></param>
		private void RemoveClient(int id)
		{
			for (int i = 0; i < activeClients.Count; i++)
				if (activeClients[i].id == id)
				{
					activeClients.RemoveAt(i);
					break;
				}
		}

		/// <summary>
		/// Assigns specified client to a Game in the gamePool
		/// </summary>
		/// <param name="passedClient"></param>
		/// <param name="gamePool"></param>
		private void AssignToGame(Client passedClient, List<Game> gamePool)
		{
			for (int i = 0; i < gamePool.Count; i++)
			{
				if (gamePool[i].playerAmount < 2)
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
					if (activeGames[i].playerAmount == 0 && activeGames.Count > 2)
					{
						activeGames.RemoveAt(i);
						i--;
					}
					else if (activeGames[i].playerAmount == 2)
					{
						activeGames.Add(new Game(gameId++));
					}
				}
			}
		}
	}
}
