#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AN.Integration._1C.Models;
using AN.Integration.DynamicsCore.Api;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AN.Integration.SyncToDynamics.Job.Extensions;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IDictionary<Type,
            Func<IOneCData, ApiRequest>> _mappers;

        public ServiceBusHandler(IDictionary<Type,
            Func<IOneCData, ApiRequest>> mappers)
        {
            _mappers = mappers;
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {
            var body = message.GetBody();

            //var mapper = _mappers.GetMapper(body);
            //var apiRequest = mapper.Invoke(body);

            await Task.CompletedTask;
        }
    }
}