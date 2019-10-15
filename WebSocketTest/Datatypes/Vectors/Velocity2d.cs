using System;

namespace WebSocketTest.Datatypes.Vectors
{
	public class Velocity2d : Vector2d
	{
		public double SpeedMultiplier { get; private set; } = 1;
		public double Speed
		{
			get => Math.Sqrt(Math.Pow(X * SpeedMultiplier, 2) + Math.Pow(X * SpeedMultiplier, 2));
		}

		public Velocity2d(int x, int y) : base(x, y) { }

		/// <summary>
		/// Increases the speed multiplier by 0.1
		/// </summary>
		public void IncreaseMultiplier()
		{
			SpeedMultiplier += 0.1;
		}
	}
}
