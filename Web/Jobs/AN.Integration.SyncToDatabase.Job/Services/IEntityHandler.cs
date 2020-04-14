using System.Threading.Tasks;
using AN.Integration.Database.Models;

namespace AN.Integration.SyncToDatabase.Job.Services
{
    public interface IEntityHandler<in TModel> where TModel : class, IDatabaseTable
    {
        Task UpsertAsync(TModel model);

        Task DeleteAsync(TModel model);
    }
}