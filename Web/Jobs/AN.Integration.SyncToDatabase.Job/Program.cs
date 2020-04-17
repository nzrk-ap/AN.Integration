using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using AN.Integration.Database.Client;
using AN.Integration.Database.Query;
using AN.Integration.SyncToDatabase.Job.Extensions;
using AN.Integration.SyncToDatabase.Job.Services;

namespace AN.Integration.SyncToDatabase.Job
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (Debugger.IsAttached)
                    {
                        builder.AddUserSecrets<Program>();
                    }
                    else
                    {
                        builder.AddEnvironmentVariables();
                    }
                })
                .ConfigureWebJobs((context, builder) =>
                {
                    builder.AddAzureStorageCoreServices();
                    builder.AddServiceBus((ops) =>
                    {
                        ops.ConnectionString = context.Configuration
                            .GetConnectionString("ServiceBus");
                    });
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IDatabaseClient>(provider =>
                    {
                        var sqlConnectionString = context.Configuration
                            .GetConnectionString("SqlDatabase");
                        return new SqlServerClient(sqlConnectionString);
                    });
                    services.RegisterEntityMappers();
                    services.AddSingleton<QueryBuilder>();
                    services.AddTransient<IHandler, EntityHandler>();
                })
                .ConfigureLogging((context, builder) => { builder.AddConsole(); });

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}