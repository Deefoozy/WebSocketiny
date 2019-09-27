using System;
using System.Collections.Generic;
using WebSocketTest.Responses;
using WebSocketTest.Datatypes;

namespace WebSocketTest.ResponseHandlers
{
	static class MessageSender
	{
		static public void SendToAll(string message, List<Client> targetClients)
		{
			byte[] byteMessage = Message.GenerateMessage(message);
			for (int i = 0; i < targetClients.Count; i++)
				try
				{
					targetClients[i].client.GetStream().Write(byteMessage, 0, byteMessage.Length);
				}
				catch
				{
					Console.WriteLine($"Error sending to| {i} | {targetClients[i].ToString()}");
				}
		}

		static public void SendToAll(string message, Player[] targetClients)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			for (int i = 0; i < targetClients.Length; i++)
				try
				{
					targetClients[i].ClientInfo.client.GetStream().Write(byteMessage, 0, byteMessage.Length);
				}
				catch
				{
					Console.WriteLine($"Error sending to| {i} | {targetClients[i].ToString()}");
				}
		}

		static public void SendToSpecific(string message, List<Client> targetClients, int id)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			for (int i = 0; i < targetClients.Count; i++)
				if (targetClients[i].id == id)
				{
					targetClients[i].client.GetStream().Write(byteMessage, 0, byteMessage.Length);
					break;
				}
		}

		static public void SendToSpecific(string message, Client targetClient)
		{
			byte[] byteMessage = Message.GenerateMessage(message);

			targetClient.client.GetStream().Write(byteMessage);
		}
	}
}
