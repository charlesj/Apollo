using System;
using System.Threading;
using System.Threading.Tasks;
using Apollo.Jobs;
using Apollo.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Apollo
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Enabled = true;
            //Logger.TraceEnabled = true;
            var host = new WebHostBuilder()
                .UseKestrel(opt => opt.AddServerHeader = false)
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

            var configuration = locator.Get<Configuration>();
            if (!configuration.IsValid())
            {
                Logger.Error("Invalid Configuration");
                Environment.Exit(1);
            }

            app.UseCors(builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                );

            app.Run(async ctx =>
            {
                Logger.Trace("Beginning Request Process");
                var context = new TestableHttpContext(ctx);
                await httpRequestProcessor.Process(context);
                Logger.Trace("Request Complete");
            });

            Logger.Info("Boot Complete!");

            Logger.Info("Starting jobs");
            var jobProcessor = locator.Get<IJobProcessor>();
            var token = new CancellationToken();
            Task.Run(() => jobProcessor.Process(token), token);
            Logger.Info("Jobs Started");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        }
    }
}
