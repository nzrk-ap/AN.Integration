using System.Threading.Tasks;
using AN.Integration.Database.Client;
using AN.Integration.Database.Models.Models;
using AN.Integration.Database.Query;

namespace AN.Integration.SyncToDatabase.Job.Services
{
    internal sealed class EntityHandler : IHandler
    {
        private readonly IDatabaseClient _databaseClient;
        private readonly QueryBuilder _queryBuilder;

        public EntityHandler(IDatabaseClient databaseClient, QueryBuilder queryBuilder)
        {
            _databaseClient = databaseClient;
            _queryBuilder = queryBuilder;
        }

        public async Task UpsertAsync(IDatabaseTable model)
        {
            //var query = _queryBuilder.GetInsertQuery(model);
            await _databaseClient.UpsertAsync(model as Contact);
        }

        public async Task DeleteAsync(IDatabaseTable model)
        {
            //var query = _queryBuilder.GetDeleteQuery(model);
            await _databaseClient.DeleteAsync(model as Contact);
        }
    }
}
