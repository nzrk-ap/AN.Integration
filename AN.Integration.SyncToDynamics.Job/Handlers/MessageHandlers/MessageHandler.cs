using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using AN.Integration.OneC.Models;
using AN.Integration.SyncToDynamics.Job.Extensions;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;
using Microsoft.Extensions.Logging;

namespace AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers
{
    public sealed class MessageHandler : IMessageHandler
    {
        private readonly IMapper _mapper;
        private readonly IDynamicsConnector _connector;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IMapper mapper, IDynamicsConnector connector,
            ILogger<MessageHandler> logger)
        {
            _mapper = mapper;
            _connector = connector;
            _logger = logger;
        }

        public async Task HandleUpsertAsync(object message)
        {
            var upsertObject = GetUpsertObject(message);
            var request = _mapper.Map<ApiRequest>(upsertObject);
            await _connector.UpsertAsync(request);

            _logger.LogInformation($"Upsert executed for {upsertObject.GetTypeName()}:{upsertObject.Code}");
        }

        public async Task HandleDeleteAsync(object message)
        {
            var deleteObject = GetDeleteObject(message);
            var request = _mapper.Map<ApiRequest>(deleteObject);
            await _connector.DeleteAsync(request);

            _logger.LogInformation($"Delete executed for {deleteObject.GetTypeName()}:{deleteObject.Code}");
        }

        private static IOneCData GetUpsertObject(object body)
        {
            return body.GetFirstPropertyValueByInterface<IOneCData>() ??
                   throw new ArgumentException($"{body.GetTypeName()} doesn't contain {nameof(IOneCData)} data");
        }

        private static IOneCData GetDeleteObject(object body)
        {
            var code = body.GetFirstPropertyValue<string>() ??
                       throw new ArgumentException($"{body.GetTypeName()} doesn't contain a code");

            return (IOneCData) Activator.CreateInstance(body.GetType()
                       .GenericTypeArguments.First(), code) ??
                   throw new Exception($"Unable to create instance of {body.GetTypeName()}");
        }
    }
}