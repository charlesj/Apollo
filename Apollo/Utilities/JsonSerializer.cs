namespace Apollo.Utilities
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        TObject Deserialize<TObject>(string json);
    }

    public class JsonSerializer
    {

    }
}