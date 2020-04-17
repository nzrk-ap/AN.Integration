using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Client
{
    public class SqlServerClient: IDatabaseClient
    {
        private readonly SqlConnection _sqlConnection;

        public SqlServerClient(string sqlConnectionString)
        {
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
            if (await _sqlConnection.UpdateAsync(singleItem) == 0)
            {
                await _sqlConnection.InsertAsync<Guid, T>(singleItem);
            }
        }

        public async Task DeleteAsync<T>(T singleItem) where T : class, IDatabaseTable
        {
            await _sqlConnection.DeleteAsync(singleItem);
        }
    }
}