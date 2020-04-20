using System.Data.SqlClient;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public sealed class AccountRepo : TableRepo<Account>
    {
        public AccountRepo(SqlConnection connection) : base(connection)
        {
        }
    }
}
