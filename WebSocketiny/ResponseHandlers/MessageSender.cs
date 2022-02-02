using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using WebSocketiny.Responses;
using WebSocketiny.Datatypes;

namespace WebSocketiny.ResponseHandlers
{
	static class MessageSender
	{
		/// <summary>
		/// Send message to all specified clients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		public static void SendToAll(string message, List<Client> targetClients)
		{
			byte[] byteMessage = Message.GenerateMessage(message);
			for (int i = 0; i < targetClients.Count; i++)
				try
				{
					targetClients[i].stream.Write(byteMessage, 0, byteMessage.Length);
				}
				catch
				{
					Console.WriteLine($"Error sending to| {i} | {targetClients[i]}");
				}
		}

		/// <summary>
		/// Send message to client if that client exists within the targetClients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		/// <param name="id"></param>
		public static void SendToSpecific(string message, Dictionary<int, Client> targetClients, int id)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			if (targetClients.TryGetValue(key: id, out Client? client))
			{
				client.stream.Write(byteMessage, 0, byteMessage.Length);
			}
		}

		/// <summary>
		/// Send message to the given client
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClient"></param>
		/// <param name="handshake"></param>
		public static void SendToSpecific(string message, Client targetClient, bool handshake = false)
		{
			byte[] byteMessage = !handshake ? Message.GenerateMessage(message) : Encoding.UTF8.GetBytes(message);

			targetClient.stream.Write(byteMessage, 0, byteMessage.Length);
		}
	}
}
