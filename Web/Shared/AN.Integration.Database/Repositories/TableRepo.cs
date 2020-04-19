using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public abstract class TableRepo<TEntity> : ITableRepo<TEntity> where TEntity: class, IDatabaseTable
    {
        protected readonly SqlConnection Connection;

        protected TableRepo(SqlConnection connection)
        {
            Connection = connection;
        }

        public abstract Task<TEntity> SelectAsync(Guid id);

        public abstract Task DeleteAsync(TEntity singleItem);

        public abstract Task UpsertAsync(TEntity singleItem);

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
