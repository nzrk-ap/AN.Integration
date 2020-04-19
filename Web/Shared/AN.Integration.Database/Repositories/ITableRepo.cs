using System;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public interface ITableRepo<TEntity> : IDisposable where TEntity : class, IDatabaseTable
    {
        Task<TEntity> SelectAsync(Guid id);

        Task UpsertAsync(TEntity singleItem);

        Task DeleteAsync(TEntity singleItem);
    }
}
