using System;

namespace Apollo.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CommandLineOptions.Parse(args);
            if (options?.Command == null)
            {
                Environment.Exit(1);
            }

            options.Command.Execute().GetAwaiter().GetResult();
        }
    }
}
