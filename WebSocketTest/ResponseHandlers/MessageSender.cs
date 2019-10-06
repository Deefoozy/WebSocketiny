using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using WebSocketTest.Responses;
using WebSocketTest.Models.Clients;
using WebSocketTest.Models.GameObjects;

namespace WebSocketTest.ResponseHandlers
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
			var byteMessage = Message.GenerateMessage(message);
            foreach (var targetClient in targetClients)
            {
                try
                {
                    targetClient.ClientTcp.GetStream().Write(byteMessage, 0, byteMessage.Length);
                }
                catch
                {
                    PrintErrorInSending(targetClient);
                }
            }

        }

        /// <summary>
        /// Send message to all specified clients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="targetClients"></param>
        public static void SendToAll(string message, Player[] targetClients)
        {
            var byteMessage = Message.GenerateMessage(message);

            foreach (var targetClient in targetClients)
            {
                try
                {
                    targetClient.ClientInfo.ClientTcp.GetStream().Write(byteMessage, 0, byteMessage.Length);
                }
                catch
                {
                    PrintErrorInSending(targetClient.ClientInfo);
                }
            }
        }

        /// <summary>
		/// Send message to ClientTcp if that ClientTcp exists within the targetClients
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClients"></param>
		/// <param name="id"></param>
		public static void SendToSpecific(string message, List<Client> targetClients, int id)
        {
            var byteMessage = Message.GenerateMessage(message);

            foreach (var targetClient in targetClients.Where(targetClient => targetClient.Id == id))
            {
                targetClient.ClientTcp.GetStream().Write(byteMessage, 0, byteMessage.Length);
            }
        }

		/// <summary>
		/// Send message to the given ClientTcp
		/// </summary>
		/// <param name="message"></param>
		/// <param name="targetClient"></param>
		public static void SendToSpecific(string message, Client targetClient, bool handshake = false)
		{
			var byteMessage = !handshake ? Message.GenerateMessage(message) : Encoding.UTF8.GetBytes(message);

			targetClient.ClientTcp.GetStream().Write(byteMessage);
		}

        private static void PrintErrorInSending(Client targetClient)
        {
            Console.WriteLine($"Error sending to| {targetClient.Id} | {targetClient.ClientTcp}");
        }
	}
}
