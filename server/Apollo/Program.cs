﻿using System;
using System.Threading.Tasks;
using Apollo.Runtime;
using Apollo.Server;

namespace Apollo
{
    public class Program
    {
        public const string CurrentVersion = Apollo.Version;

        public static void Main(string[] args)
        {
            var kernel = new Kernel();
            var serviceLocator = kernel.Boot(BootOptions.Defaults);

            var server = serviceLocator.Get<IHttpServer>();
            server.Listen();

            var runTime = serviceLocator.Get<IRuntime>();
            var runTimeContext = serviceLocator.Get<IRuntimeContext>();
            runTime.Run();

            Console.WriteLine($"Apollo {CurrentVersion} has started. Ctrl+C will exit.");

            Console.CancelKeyPress += async delegate {
                Console.Write($"Shutting down now");
                runTimeContext.End();

                while (!runTimeContext.Ended)
                {
                    Console.Write(".");
                    await Task.Delay(1000);
                }

                Environment.Exit(0);
            };

            while (true)
            {
            }
        }
    }
}