using Newtonsoft.Json;

namespace Apollo.CLI
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj, bool indent = true)
        {
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
        }
    }
}
