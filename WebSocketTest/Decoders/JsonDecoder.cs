using WebSocketTest.Datatypes;
using System.Text.Json;

namespace WebSocketTest.Decoders
{
    static class JsonDecoder
    {
        static public Inputs ParseToInputs(Inputs target)
        {
            // Converts and returns json string to Inputs object
            string json = JsonSerializer.Serialize(target);
            return JsonSerializer.Deserialize<Inputs>(json);
        }
    }
}