using Newtonsoft.Json;

namespace Apollo.Utilities
{
    public interface IJsonSerializer
    {
        string Serialize(object obj, bool indent = false);
        TObject Deserialize<TObject>(string json);
    }

    public class ApolloJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj, bool indent = false)
        {
            if (indent)
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            return JsonConvert.SerializeObject(obj);
        }

        public TObject Deserialize<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }
}
