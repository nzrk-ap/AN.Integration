using System;
using System.Threading.Tasks;
using AN.Integration.Database.Models.Models;
using Dapper;

namespace AN.Integration.Database.Repositories
{
    public sealed class ContactRepo : TableRepo<Contact>
    {
        public ContactRepo(string connectionStr) : base(connectionStr)
        {
        }

        public override async Task<Contact> SelectAsync(Guid id)
        {
           return await Connection.GetAsync<Contact>(id);
        }

        public override async Task DeleteAsync(Contact singleItem)
        {
            await Connection.DeleteAsync(singleItem);
        }

        public override async Task UpsertAsync(Contact singleItem)
        {
            if (await Connection.UpdateAsync(singleItem) != 1)
            {
                await Connection.InsertAsync<Guid, Contact>(singleItem);
            }
        }
    }
}
