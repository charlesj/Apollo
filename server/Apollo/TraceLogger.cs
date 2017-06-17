using Apollo.Utilities;
using Newtonsoft.Json;

namespace Apollo
{
    public class TraceLogger
    {
        public static bool Enabled = false;

        public static void Trace(string message, object obj = null)
        {
            if (!Enabled)
                return;
            var console = new ConsoleNotShitty();
            console.Yellow(message);
            if(obj != null)
                console.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}