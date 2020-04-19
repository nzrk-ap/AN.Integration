using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AN.Integration.Database.Models.Models;
using AN.Integration.Database.Repositories;
using AN.Integration.Mapper.Profiles;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AN.Integration.SyncToDatabase.Job.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEntityMappers(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new DynamicsToDatabaseProfile()); });
            var mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);

            return serviceCollection.AddSingleton<IDictionary<string, Func<IExtensibleDataObject, IDatabaseTable>>>(
                new Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>>()
                {
                    {"contact", entity => mapper.Map<Contact>(entity)},
                    {"product", entity => mapper.Map<Product>(entity)}
                });
        }

        public static void RegisterRepo(this IServiceCollection serviceCollection)
        {
            var assemblyTypes = Assembly.GetAssembly(typeof(ITableRepo<>)).GetTypes();

            var handlersByInterfaces = assemblyTypes
                .Select(type => (type, requestHandlers: GetImplementedRequestTypeInterfaces(type)))
                .Where(pair => pair.requestHandlers.Any())
                .SelectMany(pair => pair.requestHandlers.Select(handlerInterface => (handlerInterface, handlerImplementation: pair.type)))
                .Where(pair => !pair.handlerImplementation.IsAbstract)
                .GroupBy(pair => pair.handlerInterface, pair => pair.handlerImplementation);

            foreach (var handlerType in handlersByInterfaces)
            {
                serviceCollection.Add(new ServiceDescriptor(handlerType.Key, handlerType.Single(), ServiceLifetime.Transient));
            }

            static List<Type> GetImplementedRequestTypeInterfaces(Type type)
                => type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITableRepo<>)).ToList();
        }
    }
}