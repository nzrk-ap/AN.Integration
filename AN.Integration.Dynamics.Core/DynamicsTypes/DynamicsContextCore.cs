using System;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
    public class DynamicsContextCore
    {
        public ContextMessageType MessageType { get; set; }
        public Guid UserId { get; set; }
        public ParameterCollectionCore InputParameters { get; set; }
        public ImageCollectionCore PreEntityImages { get; set; }
        public ImageCollectionCore PostEntityImages { get; set; }
    }
}
