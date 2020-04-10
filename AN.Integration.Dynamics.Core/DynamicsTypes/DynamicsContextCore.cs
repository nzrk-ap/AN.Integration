using System;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
    public class DynamicsContextCore
    {
        public MessageTypeEnum MessageType { get; set; }
        public Guid UserId { get; set; }
        public ParameterCollectionCore InputParameters { get; set; }
        public ImageCollectionCore PreEntityImages { get; set; }
        public ImageCollectionCore PostEntityImages { get; set; }

        public enum MessageTypeEnum {
            Create = 1,
            Update,
            Delete
        } 
    }
}
