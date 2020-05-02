using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using AN.Integration.Mapper.Profiles;
using AN.Integration.DynamicsCore.DynamicsTooling;
using AN.Integration.DynamicsCore.DynamicsTooling.OAuth;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new OneCToDynamicsProfile()); });
            return serviceCollection.AddSingleton(mappingConfig.CreateMapper());
        }

        public static IServiceCollection AddDynamicsConnector(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IDynamicsConnector>(provider =>
            {
                var options = provider.GetService<IOptions<ClientOptions>>();
                return new DynamicsConnector(options.Value, new RequestConverter());
            });
        }
    }
}