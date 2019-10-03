using static System.Math;
using WebSocketTest.Datatypes.Vectors;

namespace WebSocketTest.Datatypes
{
	class Velocity2d : Vector2d
	{
		public double SpeedMultiplier { get; private set; } = 1;
		public double Speed
		{
			get => Sqrt(Pow(X * SpeedMultiplier, 2) + Pow(X * SpeedMultiplier, 2));
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
