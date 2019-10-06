using System.Threading;
using WebSocketTest.Models.Clients;
using WebSocketTest.Models.GameObjects;
using WebSocketTest.Models.Vectors;
using WebSocketTest.ResponseHandlers;

namespace WebSocketTest.Models.Games
{
	class Game
	{
		public readonly int id;
		public readonly Player[] players = new Player[2];
		// public List<Client> spectators;
		public int PlayerAmount;
		public bool IsReady;

		private Ball _ball = new Ball(new Position2d(0, 0), new Dimensions2d(0, 0), new Velocity2d(0, 0));

		public Game(int passedId)
		{
			id = passedId;
		}

		/// <summary>
		/// Adds player to game when the game has not started yet and checks if the game can be started
		/// </summary>
		/// <param name="targetClient"></param>
		/// <returns></returns>
		public bool AddPlayer(Client targetClient)
		{
			if (!IsReady)
			{
				players[PlayerAmount] = new Player(new Position2d(0, 0), new Dimensions2d(0, 0), new Velocity2d(0, 0));
				players[PlayerAmount].BindPlayer(targetClient, PlayerAmount);

				IsReady = ++PlayerAmount == 2;
				if (IsReady)
					StartGame();
			}

			return !IsReady;
		}

		/// <summary>
		/// Starts game
		/// </summary>
		public void StartGame()
		{
			MessageSender.SendToAll("pre game", players);

			Thread.Sleep(1000);

			if (IsReady)
			{
				var framecounter = 0;
				while (IsReady)
				{
					Thread.Sleep(33);
					MessageSender.SendToAll("frame 1", players);
					// Start game logic
					if (framecounter++ == 60)
						IsReady = false;
				}
			}

			MessageSender.SendToAll("game over", players);
		}
	}
}
