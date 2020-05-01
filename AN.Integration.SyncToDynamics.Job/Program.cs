using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AN.Integration.SyncToDynamics.Job.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AN.Integration.SyncToDynamics.Job
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment() && Debugger.IsAttached)
                    {
                        builder.AddUserSecrets<Program>();
                    }
                    else
                    {
                        builder.AddEnvironmentVariables();
                    }

                    const string environment = nameof(context.HostingEnvironment);
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{environment}.json", true, true);
                })
                .ConfigureWebJobs((context, builder) =>
                {
                    builder.AddAzureStorageCoreServices();
                    builder.AddServiceBus(ops =>
                    {
                        ops.ConnectionString = context.Configuration
                            .GetConnectionString("ApiExportQueue");
                    });
                })
                .ConfigureServices((context, services) =>
                {
                    services.RegisterEntityMappers();
                })
                .ConfigureLogging((context, builder) => { builder.AddConsole(); });

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}