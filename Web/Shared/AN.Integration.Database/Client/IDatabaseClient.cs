using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Client
{
    public interface IDatabaseClient
    {
        Task<T> SelectAsync<T>(Guid id) where T : class, IDatabaseTable;

        Task<IEnumerable<T>> SelectAsync<T>(string query) where T : class, IDatabaseTable;

        Task UpsertAsync<T>(T singleItem) where T : class, IDatabaseTable;

        Task DeleteAsync<T>(T singleItem) where T : class, IDatabaseTable;
    }
}
