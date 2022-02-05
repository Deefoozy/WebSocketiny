namespace WebSocketiny.DataTypes
{
	public enum OpCode : byte
	{
		Text = 1,
		Binary = 2,
		Close = 8,
		Ping = 9,
		Pong = 10,
	}
}
