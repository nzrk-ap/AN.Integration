using AN.Integration.Database.Models;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AutoMapper;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class DynamicsToDatabaseProfile : Profile
    {
        public DynamicsToDatabaseProfile()
        {
            CreateMap<EntityCore, Contact>()
                 .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                 .ForMember(d => d.FirstName, o => o.MapFrom(s => s.GetAttributeValue<string>("firstname")))
                 .ForMember(d => d.LastName, o => o.MapFrom(s => s.GetAttributeValue<string>("lastname")))
                 .ForMember(d => d.Email, o => o.MapFrom(s => s.GetAttributeValue<string>("email")))
                 .ForMember(d => d.Mobile, o => o.MapFrom(s => s.GetAttributeValue<string>("mobilephone")));
        }
    }
}
