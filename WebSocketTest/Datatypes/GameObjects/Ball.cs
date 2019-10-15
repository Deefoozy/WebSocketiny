using System;
using System.Globalization;
using WebSocketTest.Datatypes.Vectors;
using WebSocketTest.Interfaces;

namespace WebSocketTest.Datatypes.GameObjects
{
	public class Ball : GameObject
	{
		public Ball(IVector2d position, IVector2d dimensions, IVector2d velocity) : base(position, dimensions, velocity, 0) { }

		public Ball(IPhysicsData phys) : base(phys, 0) { }

		/// <summary>
		/// Reverse the X velocity
		/// </summary>
		/// <param name="offset"></param>
		public void BounceX(double offset)
		{
			Console.WriteLine(offset.ToString(CultureInfo.InvariantCulture));
			Velocity.X -= Velocity.X * 2;

			if (Velocity.Speed < MaxSpeed)
				Velocity.IncreaseMultiplier();
		}

		/// <summary>
		/// Reverse the Y velocity
		/// </summary>
		public void BounceY()
		{
			Velocity.Y -= Velocity.Y * 2;
		}

		/// <summary>
		/// Updates Position
		/// </summary>
		/// <returns></returns>
		public Position2d Move()
		{
			Position.X += (int)(Velocity.X * Velocity.SpeedMultiplier);
			Position.Y += (int)(Velocity.Y * Velocity.SpeedMultiplier);

			return Position;
		}
	}
}
