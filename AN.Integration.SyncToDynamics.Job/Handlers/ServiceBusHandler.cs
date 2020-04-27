using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class ServiceBusHandler
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameHandling = TypeNameHandling.All,
        };

        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {


            await Task.CompletedTask;
        }
    }
}