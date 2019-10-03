using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketTest.Datatypes
{
	/// <summary>
	/// A collection of 2d vectors and information relating to physics
	/// </summary>
	interface IPhysicsData
	{
		Position2d Position { get; }
		Dimensions2d Dimensions { get; }
		Velocity2d Velocity { get; }
		int MaxSpeed { get; }
	}
}
