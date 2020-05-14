using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using WebSocketTest.Decoders;
using WebSocketTest.Responses;
using WebSocketTest.Datatypes;
using WebSocketTest.ResponseHandlers;

namespace WebSocketTest.ConnectionHandlers
{
	class ClientConnection
	{
		private readonly Dictionary<int, Client> _activeClients = new Dictionary<int, Client>();

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
					MessageSender.SendToSpecific(Handshake.GenerateHandshake(data), clientData);
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
			_activeClients.Add(clientData.id, clientData);

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
					if (clientData.client.Client.Poll(1000, SelectMode.SelectRead))
					{
						// Remove the client and end WaitForMessage
						RemoveClient(clientData.id);
						clientData.ExecDisconnectCallback();
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

				clientData.ExecMessageCallback(incomingMessage.content);

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
			_activeClients.Remove(id);
		}
	}
}
