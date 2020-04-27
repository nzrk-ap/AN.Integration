using AN.Integration._1C.Models;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Mapper.Extensions;
using AN.Integration.Dynamics.Core.Models;

namespace AN.Integration.Mapper.Profiles
{
    public sealed class OneCToDynamicsProfile: DynamicsCoreProfile
    {
        public OneCToDynamicsProfile()
        {
            CreateMap<Contact, EntityCore>()
                .MapField(s => s.Name, ContactMetadata.MentorId.LogicalName);
        }
    }
}