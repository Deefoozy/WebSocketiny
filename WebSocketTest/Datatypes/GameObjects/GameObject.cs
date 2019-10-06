namespace WebSocketTest.Datatypes
{
	class GameObject : IPhysicsData
	{
		public Position2d Position { get; private set; }
		public Dimensions2d Dimensions { get; private set; }
		public Velocity2d Velocity { get; private set; }
		public int MaxSpeed { get; private set; }

		public GameObject(IVector2d position, IVector2d dimensions, IVector2d velocity)
		{
			Init(
				(Position2d)position,
				(Dimensions2d)dimensions,
				(Velocity2d)velocity
			);
		}

		public GameObject(IPhysicsData phys)
		{
			Init(
				phys.Position,
				phys.Dimensions,
				phys.Velocity
			);
		}

		private void Init(Position2d position, Dimensions2d dimensions, Velocity2d velocity)
		{
			Position = position;
			Dimensions = dimensions;
			Velocity = velocity;
		}
	}
}
