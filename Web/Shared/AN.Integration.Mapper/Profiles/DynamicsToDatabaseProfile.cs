using AN.Integration.Database.Models.Models;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;
using AutoMapper;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class DynamicsToDatabaseProfile : Profile
    {
        public DynamicsToDatabaseProfile()
        {
            CreateMap<ReferenceCore, Contact>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            CreateMap<ReferenceCore, Product>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            CreateMap<EntityCore, Contact>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.GetText("firstname")))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.GetText("lastname")))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.GetText("email")))
                .ForMember(d => d.Mobile, o => o.MapFrom(s => s.GetText("mobilephone")))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.GetText("emailaddress1")));

            CreateMap<EntityCore, Product>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.GetText("productnumber")))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.GetText("name")));
        }
    }
}