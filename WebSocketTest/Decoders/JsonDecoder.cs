using WebSocketTest.Datatypes;
using System.Text.Json;



namespace WebSocketTest.Decoders
{
	static class JsonDecoder
	{
		static public Inputs ParseToInputs(Inputs target)
		{
                string json = JsonSerializer.Serialize(target);
                // Converts and returns json string to Inputs object
                return JsonSerializer.Deserialize<Inputs>(json); 
		}
	}
}
