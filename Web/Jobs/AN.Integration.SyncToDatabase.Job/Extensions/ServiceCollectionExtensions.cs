using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AN.Integration.Database.Models.Models;
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

            return serviceCollection.AddSingleton(new Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>>()
            {
                {"contact", entity => mapper.Map<Contact>(entity)},
                {"product", entity => mapper.Map<Product>(entity)}
            });
        }
    }
}