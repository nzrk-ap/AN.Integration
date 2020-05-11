using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using AN.Integration.Mapper.Profiles;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling;
using AN.Integration.Infrastructure.Dynamics.OAuth;
using AN.Integration.OneC.Models;
using AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers;
using AN.Integration.SyncToDynamics.Job.Services;

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
            return serviceCollection.AddTransient<DynamicsOAuthService>()
                .AddTransient<IRequestConverter, RequestConverter>()
                .AddTransient<IDynamicsConnector, DynamicsConnector>();
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IDataInstance, DataInstance>()
                .AddTransient<IMessageHandler<IOneCData>, MessageHandler<IOneCData>>();
        }
    }
}