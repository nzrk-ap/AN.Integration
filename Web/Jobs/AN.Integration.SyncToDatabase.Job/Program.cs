using AN.Integration.Database;
using AN.Integration.SyncToDatabase.Job.Handlers;
using AN.Integration.Mapper.Profiles;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AN.Integration.SyncToDatabase.Job
{
    class Program
    {
        static async Task Main(string[] args)
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
                    builder.AddServiceBus((ops)=> {
                        ops.ConnectionString = context.Configuration.GetConnectionString("ServiceBus");
                    });
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient(provider =>
                    {
                        var sqlConnectionString = context.Configuration
                        .GetConnectionString("SqlDatabase");
                        return new DatabaseClient(sqlConnectionString);
                    });
                    services.AddTransient<ServiceBusHandler>();
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new DynamicsToDatabaseProfile());
                    });
                    services.AddSingleton(mappingConfig.CreateMapper());
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                });

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}
