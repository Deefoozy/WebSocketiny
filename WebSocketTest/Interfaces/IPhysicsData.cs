using WebSocketTest.Datatypes.Vectors;

namespace WebSocketTest.Interfaces
{
	/// <summary>
	/// A collection of 2d vectors and information relating to physics
	/// </summary>
	public interface IPhysicsData
	{
		Position2d Position { get; }
		Dimensions2d Dimensions { get; }
		Velocity2d Velocity { get; }
		int MaxSpeed { get; }
	}
}
