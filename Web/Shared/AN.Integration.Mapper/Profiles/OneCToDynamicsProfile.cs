using System.Collections.Generic;
using AN.Integration.DynamicsCore.Models;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;
using AN.Integration.OneC.Models;
using AutoMapper;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class OneCToDynamicsProfile : Profile
    {
        public OneCToDynamicsProfile()
        {
            CreateMap<Contact, ApiRequest>()
                .ConvertUsing(contact =>
                    new ApiRequest(ContactMetadata.EntityLogicalName, 
                        ContactMetadata.ANCode.LogicalName, contact.Code)
                    {
                        BodyAttributes = new Dictionary<string, object>
                        {
                            {ContactMetadata.FirstName.LogicalName, contact.FirstName},
                            {ContactMetadata.LastName.LogicalName, contact.LastName},
                            {ContactMetadata.ANCode.LogicalName, contact.Code}
                        }
                    });

            //CreateMap<Contact, ApiRequest>();
            //    .ConvertUsing(data =>
            //        new ApiRequest(ContactMetadata.EntityLogicalName,
            //            ContactMetadata.ANCode.LogicalName, data.Code));
        }
    }
}