using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;
using Dapper;

namespace AN.Integration.Database.Repositories
{
    public abstract class TableRepo<TEntity> : ITableRepo<TEntity> where TEntity : class, IDatabaseTable
    {
        protected readonly SqlConnection Connection;

        protected TableRepo(SqlConnection connection)
        {
            Connection = connection;
        }

        public virtual async Task<TEntity> SelectAsync(Guid id)
        {
            return await Connection.GetAsync<TEntity>(id);
        }

        public virtual async Task<IEnumerable<TEntity>> SelectItemsAsync(string whereQuery)
        {
            return await Connection.GetListAsync<TEntity>(whereQuery);
        }

        public virtual async Task DeleteAsync(TEntity singleItem)
        {
            await Connection.OpenAsync();
            await using var transaction = await Connection.BeginTransactionAsync();
            await Connection.DeleteAsync(singleItem, transaction);
            await transaction.CommitAsync();
            await Connection.CloseAsync();
        }

        public virtual async Task UpsertAsync(TEntity singleItem)
        {
            await Connection.OpenAsync();
            await using var transaction = await Connection.BeginTransactionAsync();
            if (await Connection.UpdateAsync(singleItem, transaction) != 1)
            {
                await Connection.InsertAsync<Guid, TEntity>(singleItem, transaction);
            }
            await transaction.CommitAsync();
            await Connection.CloseAsync();
        }

        #region IDisposable Support

        private bool _isDisposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_isDisposed)
                {
                    //Do your unmanaged disposing here
                    _isDisposed = true;
                }
            }
        }

        #endregion
    }
}
