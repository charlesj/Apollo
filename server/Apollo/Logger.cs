using System;
using Apollo.Utilities;
using Newtonsoft.Json;

namespace Apollo
{
    public class Logger
    {
        private static ConsoleNotShitty console = new ConsoleNotShitty();
        public static bool Enabled = false;
        public static bool TraceEnabled = false;

        public static void Info(string message, object obj = null)
        {
            Write(console.Green, $"INFO {message}", obj);
        }

        public static void Error(string message, object obj = null)
        {
            Write(console.Red, $"ERROR {message}", obj);
        }

        public static void Trace(string message, object obj = null)
        {
            if (!TraceEnabled)
                return;
            console.Write($"{DateTime.Now} ", false);
            console.Yellow($"TRACE {message}");
            WriteObject(obj);
        }

        public static void Write(Action<string, bool> output, string message, object obj)
        {
            if (!Enabled)
                return;
            console.Write($"{DateTime.Now} ", false);
            output(message, true);
            WriteObject(obj);
        }

        public static void WriteObject(object obj)
        {
            if(obj != null)
                console.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

    }
}
