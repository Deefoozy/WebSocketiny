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

		// private Ball ball = new Ball();

		public Game(int passedId)
		{
			id = passedId;
		}

		public bool AddPlayer(Client targetClient)
		{
			if (!ready)
			{
				// client with pos
				players[playerAmount] = new Player(new Vector2d(0, 0), new Vector2d(0, 0), new Vector2d(0, 0));
				// set client data
				players[playerAmount].BindPlayer(targetClient, playerAmount);

				ready = ++playerAmount == 2;
				if (ready)
				{
					StartGame();
				}
			}

			return !ready;
		}

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
