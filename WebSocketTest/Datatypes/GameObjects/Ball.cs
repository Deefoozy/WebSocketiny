using System;

namespace WebSocketTest.Datatypes
{
	class Ball : GameObject
	{
		public Ball(IVector2d pos, IVector2d dim, IVector2d vel) : base(pos, dim, vel) { }

		public Ball(IPhysicsData phys) : base(phys) { }

		public void BounceX(double offset)
		{
			Console.WriteLine(offset.ToString());
			Velocity.X -= Velocity.X * 2;

			if (Velocity.Speed < MaxSpeed)
				Velocity.IncreaseMultiplier();
		}

		public void BounceY()
		{
			Velocity.Y -= Velocity.Y * 2;
		}

		public Position2d Move()
		{
			Position.X += (int)(Velocity.X * Velocity.SpeedMultiplier);
			Position.Y += (int)(Velocity.Y * Velocity.SpeedMultiplier);

			return Position;
		}
	}
}
