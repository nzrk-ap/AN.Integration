using System.Threading.Tasks;
using AN.Integration.Database;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.SyncToDatabase.Job.Services
{
    public sealed class ProductHandler : IEntityHandler<Product>
    {
        private readonly DatabaseClient _databaseClient;

        public ProductHandler(DatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        public async Task UpsertAsync(Product model)
        {
            await _databaseClient.UpsertAsync(model);
        }

        public async Task DeleteAsync(Product model)
        {
            await _databaseClient.DeleteAsync(model);
        }
    }
}