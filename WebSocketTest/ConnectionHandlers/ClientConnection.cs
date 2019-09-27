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

		public void Accept(Client clientData)
		{
			while (clientData.client.Available < Encoding.UTF8.GetByteCount("GET"))
				Thread.Sleep(1);

			string data = Encoding.UTF8.GetString(ReadStream(clientData, true));


			if (data.StartsWith("GET"))
			{
				SendHandshake(clientData, Handshake.GenerateHandshake(data));
			}

			activeClients.Add(clientData);

			AssignToGame(clientData, activeGames);

			WaitForMessage(clientData);

			clientData.client.GetStream().Close();

			Console.WriteLine($"{clientData.id} | Closed client connection");
		}

		private void WaitForMessage(Client clientData)
		{
			bool open = true;

			while (open)
			{
				if (!clientData.client.Connected)
					return;

				// Put thread in low priority pool if no data is available
				while (!clientData.client.GetStream().DataAvailable)
				{
					// Check if client is still connected
					if (clientData.client.Client.Poll(10, SelectMode.SelectRead))
					{
						RemoveClient(clientData.id);
						break;
					}

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

		private void SendHandshake(Client clientData, byte[] handshake)
		{
			clientData.client.GetStream().Write(handshake, 0, handshake.Length);
		}

		private byte[] ReadStream(Client clientData, bool initial = false)
		{
			byte[] bytes = new byte[initial ? clientData.client.Available : 1024];
			clientData.client.GetStream().Read(bytes, 0, bytes.Length);
			return bytes;
		}

		private void RemoveClient(int id)
		{
			for (int i = 0; i < activeClients.Count; i++)
				if (activeClients[i].id == id)
				{
					activeClients.RemoveAt(i);
					break;
				}
		}

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
