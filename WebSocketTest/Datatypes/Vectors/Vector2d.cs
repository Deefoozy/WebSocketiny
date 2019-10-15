using WebSocketTest.Interfaces;

namespace WebSocketTest.Datatypes.Vectors
{
	public class Vector2d : IVector2d
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2d(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
