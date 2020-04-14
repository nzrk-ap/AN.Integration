#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AN.Integration.Database;
using AN.Integration.Database.Models;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;
using AN.Integration.Dynamics.Core.Utilities;

namespace AN.Integration.SyncToDatabase.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly DatabaseClient _databaseClient;
        private readonly Dictionary<string, Type> _handlerTypes;
        private readonly Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> _mappers;

        private readonly Dictionary<ContextMessageType, string> _handlerMethods =
            new Dictionary<ContextMessageType, string>
            {
                {ContextMessageType.Create, "UpsertAsync"},
                {ContextMessageType.Update, "UpsertAsync"},
                {ContextMessageType.Delete, "DeleteAsync"}
            };

        public ServiceBusHandler(
            DatabaseClient databaseClient, Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> mappers,
            Dictionary<string, Type> handlerTypes)
        {
            _databaseClient = databaseClient;
            _mappers = mappers;
            _handlerTypes = handlerTypes;
        }

        public Task HandleMessage(
            [ServiceBusTrigger("crm-export")] Message message, ILogger logger)
        {
            var context = ContextSerializer.ToContext(message.Body);

            var targetRef = context.GetTargetRef();
            var methodName = _handlerMethods.FirstOrDefault(h => h.Key == context.MessageType).Value;

            var handler = _handlerTypes.FirstOrDefault(h => h.Key == targetRef.LogicalName).Value;
            var instance = Activator.CreateInstance(handler, _databaseClient);
            var method = handler.GetMethod(methodName);
            var mapper = _mappers.FirstOrDefault(i => i.Key == targetRef.LogicalName).Value;

            var model = context.MessageType != ContextMessageType.Delete
                ? mapper.Invoke(context.GetTargetEntity())
                : mapper.Invoke(targetRef);

            method?.Invoke(instance, new object?[] {model});

            logger.LogInformation($"Message for {targetRef.LogicalName}:{targetRef.Id} handled");

            return Task.CompletedTask;
        }
    }
}