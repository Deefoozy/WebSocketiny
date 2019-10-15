namespace WebSocketTest.Datatypes.Clients
{
	public class Inputs
	{
		private bool _leftInput;
		private bool _rightInput;

		public void ApplyInputs(byte left, byte right)
		{
			if (left != 2)
				_leftInput = left == 1;
			if (right != 2)
				_rightInput = right == 1;
		}

		public bool Left()
		{
			return _leftInput;
		}

		public bool Right()
		{
			return _rightInput;
		}
	}
}
