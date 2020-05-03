#nullable enable
using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AN.Integration.OneC.Models;
using AN.Integration.OneC.Messages;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;
using AN.Integration.Infrastructure.Extensions;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicsConnector _connector;

        public ServiceBusHandler(IMapper mapper, IDynamicsConnector connector)
        {
            _mapper = mapper;
            _connector = connector;
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {
            var body = message.GetBody();
            var innerArg = body.GetType().GenericTypeArguments.First();

            var value = body.GetType().GetProperties()
                .Select(p => p.GetValue(body))
                .FirstOrDefault(i => i != null && i.GetType() == innerArg);

            if (!(value is IOneCData oneCData))
                throw new Exception($"Type {value?.GetType().Name} is not supported");

            var apiRequest = _mapper.Map<ApiRequest>(value);

            if (body.GetType().GetGenericTypeDefinition() == typeof(UpsertMessage<>))
            {
                await _connector.UpsertAsync(apiRequest);
            }
            else
            {
                await _connector.DeleteAsync(apiRequest);
            }

            logger.LogInformation($"Handled message for {value.GetType().Name}." +
                                  $" Code {oneCData.Code}");
        }
    }
}