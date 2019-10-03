using System.Collections.Generic;
using System.Threading;
using WebSocketTest.Datatypes.Vectors;
using WebSocketTest.ResponseHandlers;

namespace WebSocketTest.Datatypes
{
	class Game
	{
		public readonly int id;
		public readonly Player[] players = new Player[2];
		// public List<Client> spectators;
		public int playerAmount = 0;
		public bool ready = false;

		private Ball ball = new Ball(new Position2d(0, 0), new Dimensions2d(0, 0), new Velocity2d(0, 0));

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
			if (!ready)
			{
				players[playerAmount] = new Player(new Position2d(0, 0), new Dimensions2d(0, 0), new Velocity2d(0, 0));
				players[playerAmount].BindPlayer(targetClient, playerAmount);

				ready = ++playerAmount == 2;
				if (ready)
					StartGame();
			}

			return !ready;
		}

		/// <summary>
		/// Starts game
		/// </summary>
		public void StartGame()
		{
			MessageSender.SendToAll("pre game", players);

			Thread.Sleep(1000);

			if (ready)
			{
				int framecounter = 0;
				while (ready)
				{
					Thread.Sleep(33);
					MessageSender.SendToAll("frame 1", players);
					// Start game logic
					if (framecounter++ == 60)
						ready = false;
				}
			}

			MessageSender.SendToAll("game over", players);
		}
	}
}
