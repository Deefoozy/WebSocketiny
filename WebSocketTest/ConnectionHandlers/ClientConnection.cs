using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;
using WebSocketTest.Datatypes;
using WebSocketTest.Datatypes.Clients;
using WebSocketTest.ResponseHandlers;

namespace WebSocketTest.ConnectionHandlers
{
	public class ClientConnection
	{
		private readonly Dictionary<int, Client> _activeClients = new Dictionary<int, Client>();
		private readonly List<Game> _activeGames = new List<Game>();
		private int _gameId;

		public ClientConnection()
		{
			for (var i = 0; i < 5; i++)
				_activeGames.Add(new Game(_gameId++));
		}

		/// <summary>
		/// Accept a new TcpClient, by handshaking and starting to wait for messages
		/// </summary>
		/// <param name="clientData"></param>
		public void Accept(Client clientData)
		{
			// Wait untill part of the initial connection message has been received
			while (clientData.TcpClient.Available < Encoding.UTF8.GetByteCount("GET"))
				Thread.Sleep(1);

			var data = Encoding.UTF8.GetString(ReadStream(clientData, true));

			// Check if http request
			if (data.StartsWith("GET"))
			{
				try
				{
					MessageSender.SendToSpecific(Handshake.GenerateHandshake(data), clientData);
				}
				catch (Exception exception)
				{
					Console.WriteLine($"{clientData.Id} | Could not accept TcpClient connection | {exception}");
				}
			}
			else
			{
				Console.WriteLine($"{clientData.Id} | Could not accept TcpClient connection");
				return;
			}

			// Add TcpClient to active clients and assign that TcpClient to a game
			_activeClients.Add(clientData.Id, clientData);
			AssignToGame(clientData, _activeGames);

			// Start waiting for messages, does not return untill TcpClient disconnects
			WaitForMessage(clientData);

			// End connection with TcpClient
			clientData.TcpClient.GetStream().Close();
			Console.WriteLine($"{clientData.Id} | Closed TcpClient connection");
		}

		/// <summary>
		/// Starts waiting for message, returns void when TcpClient disconnects or connection has to be closed
		/// </summary>
		/// <param name="clientData"></param>
		private void WaitForMessage(Client clientData)
		{
			while (true)
			{
				// Close connection if the TcpClient disconnected
				if (!clientData.TcpClient.Connected)
					return;

				// Wait untill message data is available
				while (!clientData.TcpClient.GetStream().DataAvailable)
				{
					// Check if TcpClient is still connected
					if (clientData.TcpClient.Client.Poll(10, SelectMode.SelectRead))
					{
						// Remove the TcpClient and end WaitForMessage
						RemoveClient(clientData.Id);
						return;
					}

					// Put thread in low priority pool if no data is available
					Thread.Sleep(1);
				}

				var incomingMessage = MessageDecoder.DecodeMessage(ReadStream(clientData));

				if (incomingMessage.Close)
				{
					RemoveClient(clientData.Id);
					break;
				}

				MessageSender.SendToAll("a", _activeClients.Values.ToList());

				// DO WHATEVS

				Console.WriteLine($"{clientData.Id} | {incomingMessage.Content}");

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
		private static byte[] ReadStream(Client clientData, bool initial = false)
		{
			var bytes = new byte[initial ? clientData.TcpClient.Available : 1024];
			clientData.TcpClient.GetStream().Read(bytes, 0, bytes.Length);
			return bytes;
		}

		/// <summary>
		/// Removes TcpClient from activeClients by Id
		/// </summary>
		/// <param name="id"></param>
		private void RemoveClient(int id)
		{
			_activeClients.Remove(id);
		}

		/// <summary>
		/// Assigns specified TcpClient to a Game in the gamePool
		/// </summary>
		/// <param name="passedClient"></param>
		/// <param name="gamePool"></param>
		private void AssignToGame(Client passedClient, List<Game> gamePool)
		{
			foreach (var game in gamePool)
			{
				if (game.playerAmount < 2)
				{
					game.AddPlayer(passedClient);
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
			if (_activeGames.Count == 0)
			{
				_activeGames.Add(new Game(_gameId++));
			}
			else
			{
				for (var i = 0; i < _activeGames.Count; i++)
				{
					if (_activeGames[i].playerAmount == 0 && _activeGames.Count > 2)
					{
						_activeGames.RemoveAt(i);
						i--;
					}
					else if (_activeGames[i].playerAmount == 2)
					{
						_activeGames.Add(new Game(_gameId++));
					}
				}
			}
		}
	}
}
