using AN.Integration.Database;
using AN.Integration.Database.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;
using AN.Integration.Dynamics.Core.Utilities;

namespace AN.Integration.SyncToDatabase.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly IMapper _autoMapper;
        private readonly DatabaseClient _databaseClient;

        public ServiceBusHandler(IMapper autoMapper, DatabaseClient databaseClient)
        {
            _autoMapper = autoMapper;
            _databaseClient = databaseClient;
        }

        public async Task HandleMessage(
            [ServiceBusTrigger("crm-export")] Message message, ILogger logger)
        {
            var context = Serializer.Deserialize<DynamicsContextCore>(message.Body);
            var entity = context.PreEntityImages["Image"].Merge(context.GetTarget());

            var contact = _autoMapper.Map<Contact>(entity);
            await _databaseClient.UpsertAsync(contact);

            logger.LogInformation($"{contact.GetType().Name}:{contact.Id} is ");
        }
    }
}