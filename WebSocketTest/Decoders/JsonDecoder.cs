using WebSocketTest.Datatypes;
using System.Text.Json;
//using Newtonsoft.Json;


namespace WebSocketTest.Decoders
{
	static class JsonDecoder
	{
		static public Inputs ParseToInputs(Inputs target)
		{
            string json = JsonSerializer.Serialize(target);
            var convertedObject = JsonSerializer.Deserialize<Inputs>(json); // Converts json string to Inputs object 
            return convertedObject;
		}
	}
}
