#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;
using AN.Integration.Database.Repositories;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;
using AN.Integration.Dynamics.Core.Utilities;
using AN.Integration.SyncToDatabase.Job.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AN.Integration.SyncToDatabase.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> _mappers;

        private readonly Dictionary<ContextMessageType, string> _handlerMethods =
            new Dictionary<ContextMessageType, string>
            {
                {ContextMessageType.Create, "UpsertAsync"},
                {ContextMessageType.Update, "UpsertAsync"},
                {ContextMessageType.Delete, "DeleteAsync"}
            };

        public ServiceBusHandler(IServiceProvider serviceProvider,
            IDictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> mappers)
        {
            _serviceProvider = serviceProvider;
            _mappers = mappers;
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("crm-export")] Message message, ILogger logger)
        {
            var context = ContextSerializer.ToContext(message.Body);

            await SyncByGenericRepo(GetEntityModel(context), context.MessageType);

            logger.LogInformation($"Message for " +
                                  $"{context.GetTargetRef().LogicalName}" +
                                  $":{context.GetTargetRef().Id} handled");
        }

        private IDatabaseTable GetEntityModel(DynamicsContextCore context)
        {
            var mapper = _mappers.GetMapper(context.GetTargetRef().LogicalName);

            return context.MessageType switch
            {
                var type when type == ContextMessageType.Create ||
                              type == ContextMessageType.Update
                              => mapper.Invoke(context.PreEntityImages["Image"].Merge(context.GetTargetEntity())),
                ContextMessageType.Delete => mapper.Invoke(context.GetTargetRef()),
                _ => throw new Exception($"Message type {context.MessageType} is not supported")
            };
        }

        private async Task SyncByGenericRepo(IDatabaseTable model, ContextMessageType messageType)
        {
            var repo = _serviceProvider.GetRequiredService(typeof(ITableRepo<>)
                .MakeGenericType(model.GetType()));

            var methodName = _handlerMethods.FirstOrDefault(h => h.Key == messageType).Value;
            var method = repo.GetType().GetMethod(methodName)
                         ?? throw new Exception($"Method {methodName} is not found");

            var result = method.Invoke(repo, new object[] { model });
            if (result is null) throw new Exception($"Method {method} execution result is null");

            await (Task)result;
        }
    }
}