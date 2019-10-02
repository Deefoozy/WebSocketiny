using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketTest.Datatypes
{
	class Player : GameObject
	{
		private int _score = 0;
		private int _playerNumber;
		private int _speed = 15;
		private Inputs _inputs = new Inputs();

		public Client ClientInfo { get; private set; }

		public Player(IVector2d position, IVector2d dimensions, IVector2d velocity) : base(position, dimensions, velocity) { }

		public Player(IPhysicsData phys) : base(phys) { }

		public void BindPlayer(Client passedClient, int passedPlayerNumber)
		{
			ClientInfo = passedClient;
			_playerNumber = passedPlayerNumber;
			Position.SetPosition(
				200,
				50 + 700 * (_playerNumber - 1)
			);
		}

		public void MoveLeft()
		{
			Position.X -= _speed;
		}

		public void MoveRight()
		{
			Position.X += _speed;
		}

		public int GetScore()
		{
			return _score;
		}
	}
}
