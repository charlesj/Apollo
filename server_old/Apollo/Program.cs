using System;
using System.Data;
using System.Threading.Tasks;
using Apollo.Runtime;
using Apollo.Server;
using Dapper;

namespace Apollo
{
    public class Program
    {
        public const string CurrentVersion = Apollo.Version;

        public static void Main(string[] args)
        {
            Logger.Enabled = true;
            Logger.TraceEnabled = false;
            Logger.Info($"Apollo {CurrentVersion} is booting...");

            var kernel = new Kernel();
            var serviceLocator = kernel.Boot(BootOptions.Defaults);

            var configuration = serviceLocator.Get<IConfiguration>();
            if (!configuration.IsValid())
            {
                Logger.Error("Invalid Configuration");
                Environment.Exit(1);
            }

            var server = serviceLocator.Get<IHttpServer>();
            server.Listen();

            var runTime = serviceLocator.Get<IRuntime>();
            var runTimeContext = serviceLocator.Get<IRuntimeContext>();
            runTime.Run();

            Logger.Info($"Apollo {CurrentVersion} has started. Ctrl+C will exit.");

            Console.CancelKeyPress += delegate {
                Logger.Info($"Shutting down now");
                runTimeContext.End();

                while (!runTimeContext.Ended)
                {
                    Console.Write(".");
                    Task.Delay(100).GetAwaiter().GetResult();
                }

                Logger.Info("Shutdown complete.");
                Environment.Exit(0);
            };

            while (true)
            {
            }
        }
    }
}
