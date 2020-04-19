using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using AN.Integration.Database.Models.Models;
using AN.Integration.Database.Query;

namespace AN.Integration.Database.Client
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly SqlConnection _sqlConnection;
        private readonly QueryBuilder _queryBuilder;

        public DatabaseClient(string sqlConnectionString, QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
            _sqlConnection = new SqlConnection(sqlConnectionString);
        }

        public async Task<T> SelectAsync<T>(Guid id) where T : class, IDatabaseTable
        {
            return await _sqlConnection.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string query) where T : class, IDatabaseTable
        {
            return await _sqlConnection.GetListAsync<T>(query);
        }

        public async Task UpsertAsync<T>(T singleItem) where T : class, IDatabaseTable
        {
            var query = _queryBuilder.GetUpdateQuery(singleItem);
            if (await _sqlConnection.UpdateAsync(query) != 1)
            {
               query = _queryBuilder.GetInsertQuery(singleItem);
               await _sqlConnection.InsertAsync(query);
            }
        }

        public async Task DeleteAsync<T>(T singleItem) where T : class, IDatabaseTable
        {
            await _sqlConnection.DeleteAsync(singleItem);
        }


    }
}