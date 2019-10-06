using static System.Math;

namespace WebSocketTest.Models.Vectors
{
    internal class Velocity2d : Vector2d
	{
		public double SpeedMultiplier { get; private set; } = 1;
		public double Speed => Sqrt(Pow(X * SpeedMultiplier, 2) + Pow(X * SpeedMultiplier, 2));

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
