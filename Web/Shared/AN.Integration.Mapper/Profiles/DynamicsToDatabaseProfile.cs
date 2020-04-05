using AN.Integration.Database.Models;
using AN.Integration.Dynamics.Extensions;
using AutoMapper;
using Microsoft.Xrm.Sdk;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class DynamicsToDatabaseProfile : Profile
    {
        public DynamicsToDatabaseProfile()
        {

            CreateMap<Entity, Contact>()
                 .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                 .ForMember(d => d.FirstName, o => o.MapFrom(s => s.GetText("firstname")))
                 .ForMember(d => d.LastName, o => o.MapFrom(s => s.GetText("lastname")))
                 .ForMember(d => d.Email, o => o.MapFrom(s => s.GetText("email")))
                 .ForMember(d => d.Mobile, o => o.MapFrom(s => s.GetText("mobilephone")));
        }
    }
}
