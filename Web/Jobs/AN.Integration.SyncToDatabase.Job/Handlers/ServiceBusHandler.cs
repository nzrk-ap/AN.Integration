#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AN.Integration.Database.Client;
using AN.Integration.Database.Models.Models;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;
using AN.Integration.Dynamics.Core.Utilities;
using AN.Integration.SyncToDatabase.Job.Services;

namespace AN.Integration.SyncToDatabase.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IDatabaseClient _databaseClient;
        private readonly IHandler _entityHandler;
        private readonly Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> _mappers;

        private readonly Dictionary<ContextMessageType, string> _handlerMethods =
            new Dictionary<ContextMessageType, string>
            {
                {ContextMessageType.Create, "UpsertAsync"},
                {ContextMessageType.Update, "UpsertAsync"},
                {ContextMessageType.Delete, "DeleteAsync"}
            };

        public ServiceBusHandler(IDatabaseClient databaseClient, IHandler entityHandler,
            Dictionary<string, Func<IExtensibleDataObject, IDatabaseTable>> mappers)
        {
            _databaseClient = databaseClient;
            _entityHandler = entityHandler;
            _mappers = mappers;
        }

        public Task HandleMessage(
            [ServiceBusTrigger("crm-export")] Message message, ILogger logger)
        {
            var context = ContextSerializer.ToContext(message.Body);
            var targetRef = context.GetTargetRef();
            var methodName = _handlerMethods.FirstOrDefault(h => h.Key == context.MessageType).Value;
           
            var method = _entityHandler.GetType().GetMethod(methodName);
            var mapper = _mappers.FirstOrDefault(i => i.Key == targetRef.LogicalName).Value;

            var model = context.MessageType != ContextMessageType.Delete
                ? mapper.Invoke(context.PreEntityImages["Image"].Merge(context.GetTargetEntity()))
                : mapper.Invoke(targetRef);

            method?.Invoke(_entityHandler, new object[] {model});
            logger.LogInformation($"Message for {targetRef.LogicalName}:{targetRef.Id} handled");

            return Task.CompletedTask;
        }
    }
}