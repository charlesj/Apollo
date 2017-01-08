using System;
using Newtonsoft.Json;

namespace Apollo.Server
{
    public interface IJsonRpcRequestLogger
    {
        void Info(string message, object context = null);
        void Error(string message, object context = null);
    }

    public class JsonRpcRequestLogger : IJsonRpcRequestLogger
    {
        public void Info(string message, object context = null)
        {
            Console.WriteLine($"INFO {message}");
            if(context != null)
                Console.WriteLine(JsonConvert.SerializeObject(context, Formatting.Indented));
        }

        public void Error(string message, object context = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR {message}");
            Console.ResetColor();
            if(context != null)
                Console.WriteLine(JsonConvert.SerializeObject(context, Formatting.Indented));
        }
    }
}