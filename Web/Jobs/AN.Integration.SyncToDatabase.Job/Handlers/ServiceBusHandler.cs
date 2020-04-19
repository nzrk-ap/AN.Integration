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
            var targetRef = context.GetTargetRef();

            var mapper = _mappers.FirstOrDefault(i => i.Key == targetRef.LogicalName).Value;
            var model = context.MessageType != ContextMessageType.Delete
                ? mapper.Invoke(context.PreEntityImages["Image"].Merge(context.GetTargetEntity()))
                : mapper.Invoke(targetRef);

            await SyncByGenericRepo(model, context.MessageType);

            logger.LogInformation($"Message for {targetRef.LogicalName}:{targetRef.Id} handled");
        }

        private async Task SyncByGenericRepo(IDatabaseTable model, ContextMessageType messageType)
        {
            var handlerType = typeof(ITableRepo<>).MakeGenericType(model.GetType());
            var repo = _serviceProvider.GetService(handlerType);

            var methodName = _handlerMethods.FirstOrDefault(h => h.Key == messageType).Value;
            var method = repo.GetType().GetMethod(methodName)
                         ?? throw new Exception($"Method {methodName} is not found");
            var result = method.Invoke(repo, new object[] { });
            if (result is null) throw new Exception($"Method {method} execution result is null");

            await ((Task)result);
        }
    }
}