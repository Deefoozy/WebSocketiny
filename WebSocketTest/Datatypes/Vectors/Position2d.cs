using WebSocketTest.Datatypes.Vectors;

namespace WebSocketTest.Datatypes
{
	class Position2d : Vector2d
	{
		public Position2d(int x, int y) : base(x, y) { }

		public void SetPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
