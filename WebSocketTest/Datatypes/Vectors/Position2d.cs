namespace WebSocketTest.Datatypes.Vectors
{
	public class Position2d : Vector2d
	{
		public Position2d(int x, int y) : base(x, y) { }

		/// <summary>
		/// Sets position to the provided coordinates
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
