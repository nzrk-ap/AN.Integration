using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public sealed class ProductRepo: TableRepo<Product>
    {
        public ProductRepo(SqlConnection connection) : base(connection)
        {
        }

        public override Task<Product> SelectAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task UpsertAsync(Product singleItem)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Product singleItem)
        {
            throw new NotImplementedException();
        }
    }
}