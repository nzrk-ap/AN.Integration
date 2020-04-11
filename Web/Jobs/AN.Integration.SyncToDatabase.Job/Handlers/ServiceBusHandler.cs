using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using AN.Integration.Database;
using AN.Integration.Database.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;

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
            var context = DeserializeContext(message.Body);
            var entity = context.PreEntityImages["Image"].Merge(context.GetTarget());

            var contact = _autoMapper.Map<Contact>(entity);
            await _databaseClient.UpsertAsync(contact);

            logger.LogInformation($"{contact.GetType().Name}:{contact.Id} is ");
        }

        private static DynamicsContextCore DeserializeContext(byte[] data)
        {
            var knownTypes = new List<Type>()
            {
                typeof(EntityCore),
                typeof(ReferenceCore),
            };

            var serializerSettings = new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            };

            DynamicsContextCore deserializedObject;
            var serializer = new DataContractJsonSerializer(typeof(DynamicsContextCore), serializerSettings);
            using (var stream = new MemoryStream(data))
            {
                deserializedObject = (DynamicsContextCore)serializer.ReadObject(stream);
            }

            return deserializedObject;
        }
    }
}