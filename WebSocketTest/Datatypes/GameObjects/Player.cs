namespace WebSocketTest.Datatypes
{
	class Player : GameObject
	{
		private Inputs _inputs = new Inputs();
		private int _playerNumber;
		private readonly int _score = 0;
		private readonly int _speed = 15;

		public Player(IVector2d position, IVector2d dimensions, IVector2d velocity) : base(position, dimensions,
			velocity)
		{
		}

		public Player(IPhysicsData phys) : base(phys) { }

		public Client ClientInfo { get; private set; }

		/// <summary>
		///     Binds client to player
		/// </summary>
		/// <param name="passedClient"></param>
		/// <param name="passedPlayerNumber"></param>
		public void BindPlayer(Client passedClient, int passedPlayerNumber)
		{
			ClientInfo = passedClient;
			_playerNumber = passedPlayerNumber;
			Position.SetPosition(
				200,
				50 + 700 * (_playerNumber - 1)
			);
		}

		/// <summary>
		///     Moves player to the left
		/// </summary>
		public void MoveLeft()
		{
			Position.X -= _speed;
		}

		/// <summary>
		///     Moves player to the right
		/// </summary>
		public void MoveRight()
		{
			Position.X += _speed;
		}

		/// <summary>
		///     Gets player score
		/// </summary>
		/// <returns></returns>
		public int GetScore()
		{
			return _score;
		}
	}
}
