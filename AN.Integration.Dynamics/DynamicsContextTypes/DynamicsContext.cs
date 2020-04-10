using System;
using AN.Integration.Dynamics.Utilities;

namespace AN.Integration.Dynamics.DynamicsContextTypes
{
    public class DynamicsContext
    {
        public MessageTypeEnum MessageType { get; set; }
        public Guid UserId { get; set; }
        public DynamicsParameterCollection InputParameters { get; set; }
        public DynamicsImageCollection PreEntityImages { get; set; }
        public DynamicsImageCollection PostEntityImages { get; set; }

        public enum MessageTypeEnum {
            Create = 1,
            Update,
            Delete
        } 
    }
}
