namespace WebSocketTest.Datatypes.Vectors
{
	class Vector2d : IVector2d
	{
		public Vector2d(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }
	}
}
