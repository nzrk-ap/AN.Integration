using AN.Integration.Database.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AN.Integration.Database.Query;

namespace AN.Integration.Database
{
    public class DatabaseClient
    {
        private readonly QueryBuilder _queryBuilder = new QueryBuilder();
        private readonly SqlConnection _sqlConnection;

        public DatabaseClient(string sqlConnectionString)
        {
            _sqlConnection = new SqlConnection(sqlConnectionString);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string query) where T : IDatabaseTable
        {
            return await _sqlConnection.QueryAsync<T>(query);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(QueryFilter filter) where T : IDatabaseTable
        {
            var query = _queryBuilder.GetQuery<T>(filter);
            return await _sqlConnection.QueryAsync<T>(query);
        }

        public async Task UpsertAsync<T>(T singleItem) where T : class, IDatabaseTable
        {
            if (!await _sqlConnection.UpdateAsync(singleItem))
            {
                _sqlConnection.Insert(singleItem);
            }
        }
    }
}
