using AN.Integration.Database.Models.Models;
using AN.Integration.Mapper.Extensions;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class DynamicsToDatabaseProfile : DynamicsCoreProfile
    {
        public DynamicsToDatabaseProfile()
        {
            CreateEntityMap<Product>()
                .MapField(s => s.Id, d => d.Id)
                .MapField("productnumber", d => d.ProductId)
                .MapField("name", d => d.Name);

            CreateEntityMap<Account>()
                .MapField(s => s.Id, d => d.Id)
                .MapField("name", d => d.Name);

            CreateEntityMap<Contact>()
                .MapField(s => s.Id, d => d.Id)
                .MapField("firstname", d => d.FirstName)
                .MapField("lastname", d => d.LastName)
                .MapField("email", d => d.Email)
                .MapField("mobilephone", d => d.Mobile)
                .MapField("emailaddress1", d => d.Email)
                .MapField("an_accountid", d => d.AccountId);
        }
    }
}