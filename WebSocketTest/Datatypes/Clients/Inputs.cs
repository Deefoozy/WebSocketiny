namespace WebSocketTest.Datatypes
{
	class Inputs
	{
		private bool leftInput;
		private bool rightInput;

		public void ApplyInputs(byte left, byte right)
		{
			if (left != 2)
				leftInput = left == 1;
			if (right != 2)
				rightInput = right == 1;
		}

		public bool Left()
		{
			return leftInput;
		}

		public bool Right()
		{
			return rightInput;
		}
	}
}
