using System.Data.SqlClient;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Repositories
{
    public sealed class ContactRepo : TableRepo<Contact>
    {
        public ContactRepo(SqlConnection connection) : base(connection)
        {
        }
    }
}
