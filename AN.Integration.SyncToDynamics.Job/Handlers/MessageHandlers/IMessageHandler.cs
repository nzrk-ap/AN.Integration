using System.Threading.Tasks;

namespace AN.Integration.SyncToDynamics.Job.Handlers.MessageHandlers
{
    public interface IMessageHandler
    {
        Task HandleUpsertAsync(object message);

        Task HandleDeleteAsync(object message);
    }
}