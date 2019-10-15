using System;
using System.Collections.Generic;
using System.Text;
using WebSocketTest.Datatypes;
using WebSocketTest.Responses;

namespace WebSocketTest.ResponseHandlers
{
	static class MessageSender
	{
		/// <summary>
		///     Send message to all specified clients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		public static void SendToAll(string message, List<Client> targetClients)
		{
			byte[] byteMessage = Message.GenerateMessage(message);
			for (int i = 0; i < targetClients.Count; i++)
				try
				{
					targetClients[i].client.GetStream().Write(byteMessage, 0, byteMessage.Length);
				}
				catch
				{
					Console.WriteLine($"Error sending to| {i} | {targetClients[i]}");
				}
		}

		/// <summary>
		///     Send message to all specified clients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		public static void SendToAll(string message, Player[] targetClients)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			for (int i = 0; i < targetClients.Length; i++)
				try
				{
					targetClients[i].ClientInfo.client.GetStream().Write(byteMessage, 0, byteMessage.Length);
				}
				catch
				{
					Console.WriteLine($"Error sending to| {i} | {targetClients[i]}");
				}
		}

		/// <summary>
		///     Send message to client if that client exists within the targetClients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		/// <param name="id"></param>
		public static void SendToSpecific(string message, List<Client> targetClients, int id)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			for (int i = 0; i < targetClients.Count; i++)
				if (targetClients[i].id == id)
				{
					targetClients[i].client.GetStream().Write(byteMessage, 0, byteMessage.Length);
					break;
				}
		}

		/// <summary>
		///     Send message to the given client
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClient"></param>
		public static void SendToSpecific(string message, Client targetClient, bool handshake = false)
		{
			byte[] byteMessage = !handshake ? Message.GenerateMessage(message) : Encoding.UTF8.GetBytes(message);

			targetClient.client.GetStream().Write(byteMessage);
		}
	}
}
