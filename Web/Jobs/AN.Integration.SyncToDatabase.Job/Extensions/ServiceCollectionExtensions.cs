using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AN.Integration.Database.Models;
using AN.Integration.Database.Models.Models;
using AN.Integration.Mapper.Profiles;
using AN.Integration.SyncToDatabase.Job.Services;
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

            return serviceCollection.AddSingleton(new Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>>()
            {
                {"contact", entity => mapper.Map<Contact>(entity)},
                {"product", entity => mapper.Map<Product>(entity)}
            });
        }

        public static IServiceCollection RegisterEntityHandler(this IServiceCollection serviceCollection)
        {
            var types = typeof(Program).Assembly.GetTypes();

            var handlers = new Dictionary<string, Type>()
            {
                {"contact", types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IEntityHandler<Contact>)))},
                {"product", types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IEntityHandler<Product>)))}
            };

            return serviceCollection.AddSingleton(handlers);
        }
    }
}