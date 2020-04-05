using AN.Integration.Database;
using AN.Integration.Database.Models;
using AN.Integration.Models.Dynamics;
using AN.Integration.Dynamics.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

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
            [ServiceBusTrigger("crm-export")] Message message,
            ILogger logger)
        {
            logger.LogInformation(message.ContentType);

            DynamicsContext context = default;
            var serializer = new DataContractJsonSerializer(typeof(DynamicsContext));
            using (var stream = new MemoryStream(message.Body))
            {
                context = (DynamicsContext)serializer.ReadObject(stream);
            }

            var target = (Entity)context.InputParameters["Target"];
            var entity = target.Merge(context.PreEntityImages["Image"]);

            var contact = _autoMapper.Map<Contact>(entity);
            await _databaseClient.InsertAsync(contact);
        }
    }
}
