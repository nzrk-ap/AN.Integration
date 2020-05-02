#nullable enable
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AN.Integration.DynamicsCore.Api;
using AN.Integration.DynamicsCore.DynamicsTooling;
using AN.Integration.Infrastructure.Extensions;
using AN.Integration.OneC.Messages;
using AN.Integration.OneC.Models;
using AutoMapper;

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

            var apiRequest = _mapper.Map<ApiRequest>(value);

            if (body.GetType().GetGenericTypeDefinition() == typeof(UpsertMessage<>))
            {
                await _connector.UpsertAsync(apiRequest);
            }
            else
            {
                await _connector.DeleteAsync(apiRequest);
            }

            var oneCData = value as IOneCData;

            logger.LogInformation($"Handled message for {value?.GetType().Name}." +
                                  $" Code {oneCData?.Code}");

            await Task.CompletedTask;
        }
    }
}