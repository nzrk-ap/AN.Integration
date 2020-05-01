using System.Collections.Generic;
using AN.Integration._1C.Models;
using AN.Integration.DynamicsCore.Api;
using AN.Integration.DynamicsCore.Models;
using AutoMapper;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class OneCToDynamicsProfile : Profile
    {
        public OneCToDynamicsProfile()
        {
            CreateMap<Contact, ApiRequest>()
                .ConvertUsing(contact =>
                    new ApiRequest(ContactMetadata.EntityLogicalName)
                    {
                        BodyAttributes = new Dictionary<string, object>
                        {
                            {ContactMetadata.FirstName.LogicalName, contact.FirstName},
                            {ContactMetadata.LastName.LogicalName, contact.LastName},
                            {ContactMetadata.SbContactIdNumber.LogicalName, contact.Code}
                        }
                    });
        }
    }
}