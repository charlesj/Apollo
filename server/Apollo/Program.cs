using Apollo.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Apollo
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Enabled = true;
            Logger.TraceEnabled = true;
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8042/")
                .Build();

            host.Run();
        }
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            Logger.Info("Booting...");

            var kernel = new Kernel();
            var locator = kernel.Boot(BootOptions.Defaults);
            var httpRequestProcessor = locator.Get<IHttpRequestProcessor>();

            app.Run(async ctx =>
            {
                Logger.Trace("Beginning Request Process");
                var context = new TestableHttpContext(ctx);
                await httpRequestProcessor.Process(context);
                Logger.Trace("Request Complete");
            });

            Logger.Info("Boot Complete!");
        }
    }
}
