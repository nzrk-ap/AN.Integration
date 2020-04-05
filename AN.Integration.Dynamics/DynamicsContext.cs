using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace AN.Integration.Models.Dynamics
{
    public class DynamicsContext
    {
        public MessageTypeEnum MessageType { get; set; }
        public Guid UserId { get; set; }
        public ParameterCollection InputParameters { get; set; }
        public EntityImageCollection PreEntityImages { get; set; }
        public EntityImageCollection PostEntityImages { get; set; }

        public enum MessageTypeEnum {
            Create = 1,
            Update,
            Delete
        } 
    }
}
