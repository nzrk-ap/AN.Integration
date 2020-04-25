using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AN.Integration.SyncToDynamics.Job.Handlers
{
    public class ServiceBusHandler
    {
        public async Task HandleMessage(
            [ServiceBusTrigger("api-export")] Message message, ILogger logger)
        {
            await Task.CompletedTask;
        }
    }
}