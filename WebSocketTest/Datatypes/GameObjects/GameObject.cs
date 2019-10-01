using System;
using System.Collections.Generic;
using System.Text;
using WebSocketTest.Datatypes;

namespace WebSocketTest.Datatypes
{
	class GameObject : IPhysicsData
	{
		public Position2d Position { get; private set; }
		public Dimensions2d Dimensions { get; private set; }
		public Velocity2d Velocity { get; private set; }
		public int MaxSpeed { get; private set; }

		public GameObject(IVector2d pos, IVector2d dim, IVector2d vel)
		{
			Init(
				(Position2d)pos,
				(Dimensions2d)dim,
				(Velocity2d)vel
			);
		}

		public GameObject(IPhysicsData phys)
		{
			Init(
				phys.Position,
				phys.Dimensions,
				phys.Velocity
			);
		}

		private void Init(Position2d pos, Dimensions2d dim, Velocity2d vel)
		{
			Position = pos;
			Dimensions = dim;
			Velocity = vel;
		}
	}
}
