using AN.Integration.Database.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AN.Integration.Database
{
    public class DatabaseClient
    {
        private readonly QueryBuilder _queryBuilder = new QueryBuilder();
        private SqlConnection _sqlConnection;

        public DatabaseClient(string sqlConnectionString)
        {
            _sqlConnection = new SqlConnection(sqlConnectionString);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(QueryFilter filter) where T : IDatabaseTable
        {
            var query = _queryBuilder.GetQuery<T>(filter);
            return await _sqlConnection.QueryAsync<T>(query);
        }

        public async Task InsertAsync<T>(T singleItem) where T : class, IDatabaseTable
        {
            if (!await _sqlConnection.UpdateAsync(singleItem))
            {
                _sqlConnection.Insert(singleItem);
            };
        }

        private async Task InitializeTables()
        {

            await _sqlConnection.OpenAsync();
        }
    }
}
