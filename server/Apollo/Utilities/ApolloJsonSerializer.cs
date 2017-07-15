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
            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            if (indent)
                return JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings);
            return JsonConvert.SerializeObject(obj,serializerSettings);
        }

        public TObject Deserialize<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }
}
