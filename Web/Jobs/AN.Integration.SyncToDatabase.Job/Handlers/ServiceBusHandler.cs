#nullable disable
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public Task HandleMessage(
            [ServiceBusTrigger("crm-export")] Message message, ILogger logger)
        {
            var context = ContextSerializer.ToContext(message.Body);
            var targetRef = context.GetTargetRef();
            var mapper = _mappers.FirstOrDefault(i => i.Key == targetRef.LogicalName).Value;
            var model = context.MessageType != ContextMessageType.Delete
                ? mapper.Invoke(context.PreEntityImages["Image"].Merge(context.GetTargetEntity()))
                : mapper.Invoke(targetRef);

            var repoType = typeof(ITableRepo<>).MakeGenericType(typeof(TableRepo<Contact>));
            var service =_serviceProvider.GetService(repoType);

            var methodName = _handlerMethods.FirstOrDefault(h => h.Key == context.MessageType).Value;
            var repo = typeof(Program).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITableRepo<IDatabaseTable>))) 
                       ?? throw new Exception($"Repo for {model.GetType().Name} is not found");
            ////var instance = Activator.CreateInstance(repo, 
            ////    _serviceProvider.GetRequiredService<SqlConnection>());

            ////var method = repo.GetMethod(methodName);


            ////method?.Invoke(instance, new object[] {model});
            logger.LogInformation($"Message for {targetRef.LogicalName}:{targetRef.Id} handled");

            return Task.CompletedTask;
        }
    }
}