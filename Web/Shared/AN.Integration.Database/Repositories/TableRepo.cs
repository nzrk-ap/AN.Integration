using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public abstract class TableRepo<TEntity> : ITableRepo<TEntity> where TEntity: class, IDatabaseTable
    {
        protected readonly SqlConnection Connection;

        protected TableRepo(string connectionStr)
        {
            Connection = new SqlConnection(connectionStr);
        }


        public abstract Task<TEntity> SelectAsync(Guid id);

        public abstract Task DeleteAsync(TEntity singleItem);

        public abstract Task UpsertAsync(TEntity singleItem);
        
        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected TableRepo(SqlConnection connection)
        {
            Connection = connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Dispose(true);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TableRepo()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
