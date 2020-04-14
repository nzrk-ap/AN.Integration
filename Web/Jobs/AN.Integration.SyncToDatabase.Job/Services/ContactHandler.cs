using System.Threading.Tasks;
using AN.Integration.Database;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.SyncToDatabase.Job.Services
{
    public sealed class ContactHandler : IEntityHandler<Contact>
    {
        private readonly DatabaseClient _databaseClient;

        public ContactHandler(DatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        public async Task UpsertAsync(Contact model)
        {
            await _databaseClient.UpsertAsync(model);
        }

        public async Task DeleteAsync(Contact model)
        {
            await _databaseClient.DeleteAsync(model);
        }
    }
}