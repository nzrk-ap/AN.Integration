#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using AN.Integration.Infrastructure.Extensions;
using AN.Integration.OneC.Messages;
using AN.Integration.OneC.Models;
using AN.Integration.SyncToDynamics.Job.Extensions;
using AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class OneCDataHandler
    {
        private readonly IDictionary<Type, Func<object, Task>> _handlingActions;

        public OneCDataHandler(IMessageHandler<IOneCData> messageHandler)
        {
            _handlingActions = new Dictionary<Type, Func<object, Task>>
            {
                {typeof(UpsertMessage<>), messageHandler.HandleUpsertAsync},
                {typeof(DeleteMessage<>), messageHandler.HandleDeleteAsync}
            };
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {
            var body = message.GetBody();
            var genericType = body.GetType().GenericTypeArguments.SingleOrDefault();

            if (genericType is null)
            {
                logger.LogWarning($"Message of {body.GetTypeName()} doesn't contain generic argument");
                return;
            }

            try
            {
                await HandleAsync(message, body);
            }
            catch (Exception e)
            {
                logger.LogError($"Error for {genericType.Name}\n{e.Message}\n{e.StackTrace}");
            }
        }

        private async Task HandleAsync(Message message, object body)
        {
            var messageType = body.GetType().GetGenericTypeDefinition();
            var action = _handlingActions.FirstOrDefault(i => i.Key == messageType).Value
                         ?? throw new Exception($"Message type {message.GetTypeName()} is not supported");

            await action.Invoke(body);
        }
    }
}