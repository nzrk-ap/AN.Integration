using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AN.Integration.SyncToDynamics.Job.Extensions;
using AN.Integration.DynamicsCore.DynamicsTooling.OAuth;

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
                    services.Configure<ClientOptions>(context.Configuration
                        .GetSection("DynamicsClientOptions"));
                    services.AddMapper();
                    services.AddDynamicsConnector();
                    services.AddMessageHandlers();
                })
                .ConfigureLogging((context, builder) => { builder.AddConsole(); });

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}