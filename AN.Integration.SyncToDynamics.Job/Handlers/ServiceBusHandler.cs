#nullable enable
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
using System;
using AN.Integration.SyncToDynamics.Job.Extensions;

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
            var genericArg = body.GetType().GenericTypeArguments.SingleOrDefault();

            if (genericArg is null)
            {
                logger.LogWarning($"Message of {body.GetType().Name} doesn't contain generic argument");
                return;
            }

            try
            {
                var code = await SentRequestAsync(body, logger);
                logger.LogInformation($"Handled message for {genericArg.Name}:{code}");
            }
            catch (Exception e)
            {
                logger.LogError($"Error for {genericArg.Name}\n{e.Message}\n{e.StackTrace}");
            }
        }

        private async Task<string> SentRequestAsync(object message, ILogger logger)
        {
            switch (message.GetType().GetGenericTypeDefinition())
            {
                case { } type when type == typeof(UpsertMessage<>):
                    {
                        var upsertObject = GetUpsertObject(message);
                        await _connector.UpsertAsync(_mapper.Map<ApiRequest>(upsertObject));
                        return upsertObject.Code;
                    }

                case { } type when type == typeof(DeleteMessage<>):
                    {
                        var deleteObject = GetDeleteObject(message);
                        await _connector.DeleteAsync(_mapper.Map<ApiRequest>(deleteObject));
                        return deleteObject.GetFirstPropertyValue<string>();
                    }
                default:
                    logger.LogWarning($"Message type {message.GetType().Name} is not supported");
                    return string.Empty;
            }
        }

        private static IOneCData GetUpsertObject(object body)
        {
            return body.GetFirstPropertyValueByInterface<IOneCData>() ??
                   throw new ArgumentException($"Body doesn't contain {nameof(IOneCData)} data");
        }

        private static object GetDeleteObject(object body)
        {
            var code = body.GetFirstPropertyValue<string>() ??
                       throw new ArgumentException("Body doesn't contain a code");

            return Activator.CreateInstance(body.GetType()
                       .GenericTypeArguments.First(), code) ??
                   throw new Exception($"Unable to create instance of {body.GetType().Name}");
        }
    }
}