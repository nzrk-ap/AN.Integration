using System;
using System.Collections.Generic;
using AN.Integration._1C.Models;
using AN.Integration.DynamicsCore.Api;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using AN.Integration.Mapper.Profiles;
using _1C_Contact = AN.Integration._1C.Models.Contact;
using _1C_Product = AN.Integration.Models._1C.Dto.Product;

namespace AN.Integration.SyncToDynamics.Job.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEntityMappers(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new OneCToDynamicsProfile()); });
            var mapper = mappingConfig.CreateMapper();
            return serviceCollection.AddSingleton<IDictionary<Type, Func<IOneCData, ApiRequest>>>(
                new Dictionary<Type, Func<IOneCData, ApiRequest>>()
                {
                    {typeof(_1C_Contact), contact => mapper.Map<ApiRequest>(contact)},
                    {typeof(_1C_Product), product => mapper.Map<ApiRequest>(product)}
                });
        }
    }
}