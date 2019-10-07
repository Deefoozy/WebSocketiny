using System.Collections;
using WebSocketTest.Datatypes;
using System.Text.Json;
using System.Collections.Generic;

namespace WebSocketTest.Decoders
{
	static class JsonDecoder
	{
		static public Inputs ParseToInputs(Inputs target)
		{
			return new Inputs();
		}

        static public IDictionary JsonToDictionary(string json)
        {
            var options = new JsonSerializerOptions { AllowTrailingCommas = true };
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json, options);
        }
	}
}
