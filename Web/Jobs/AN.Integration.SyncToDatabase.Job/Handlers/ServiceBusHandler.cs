using AN.Integration.Database;
using AN.Integration.Database.Models;
using AN.Integration.Models.Dynamics;
using AN.Integration.Dynamics.Utilities;
using AN.Integration.Dynamics.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;

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
            var context = Serializer.Deserialize<DynamicsContext>(message.Body);
            var entity = context.GetTarget().Merge(context.PreEntityImages["Image"]);

            var contact = _autoMapper.Map<Contact>(entity);
            await _databaseClient.UpsertAsync(contact);

            logger.LogInformation($"{contact.GetType().Name}:{contact.Id} is ");
        }
    }
}