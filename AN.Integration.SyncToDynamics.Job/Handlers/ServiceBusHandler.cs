#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using AN.Integration.Infrastructure.Extensions;
using AN.Integration.OneC.Messages;
using AN.Integration.SyncToDynamics.Job.Extensions;
using AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IMessageHandler _messageHandler;

        public ServiceBusHandler(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {
            var body = message.GetBody();
            var genericArg = body.GetType().GenericTypeArguments.SingleOrDefault();

            if (genericArg is null)
            {
                logger.LogWarning($"Message of {body.GetTypeName()} doesn't contain generic argument");
                return;
            }

            try
            {
                await HandleMessageAsync(body, logger);
            }
            catch (Exception e)
            {
                logger.LogError($"Error for {genericArg.Name}\n{e.Message}\n{e.StackTrace}");
            }
        }

        private async Task HandleMessageAsync(object message, ILogger logger)
        {
            switch (message.GetType().GetGenericTypeDefinition())
            {
                case { } type when type == typeof(UpsertMessage<>):
                    await _messageHandler.HandleUpsertAsync(message);
                    break;
                case { } type when type == typeof(DeleteMessage<>):
                    await _messageHandler.HandleDeleteAsync(message);
                    break;
                default:
                    logger.LogWarning($"Message type {message.GetTypeName()} is not supported");
                    break;
            }
        }
    }
}