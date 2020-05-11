using AutoMapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AN.Integration.SyncToDynamics.Job.Services;
using AN.Integration.SyncToDynamics.Job.Extensions;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;

namespace AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers
{
    public sealed class MessageHandler<T> : IMessageHandler<T>
    {
        private readonly IMapper _mapper;
        private readonly IDataInstance _dataInstance;
        private readonly IDynamicsConnector _connector;
        private readonly ILogger<MessageHandler<T>> _logger;

        public MessageHandler(IMapper mapper, IDataInstance dataInstance,
            IDynamicsConnector connector, ILogger<MessageHandler<T>> logger)
        {
            _mapper = mapper;
            _dataInstance = dataInstance;
            _connector = connector;
            _logger = logger;
        }

        public async Task HandleUpsertAsync(object message)
        {
            var upsertObject = _dataInstance.GetInstanceForUpsert<T>(message);
            var request = _mapper.Map<ApiRequest>(upsertObject);
            await _connector.UpsertAsync(request);

            _logger.LogInformation($"Upsert executed for {request.EntityName}:{request.KeyValue}");
        }

        public async Task HandleDeleteAsync(object message)
        {
            var deleteObject = _dataInstance.GetInstanceForDelete<T>(message);
            var request = _mapper.Map<ApiRequest>(deleteObject);
            await _connector.DeleteAsync(request);

            _logger.LogInformation($"Delete executed for {request.EntityName}:{request.KeyValue}");
        }
    }
}