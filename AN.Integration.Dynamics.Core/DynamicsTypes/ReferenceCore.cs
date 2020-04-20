using System;
using System.Runtime.Serialization;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
    [DataContract]
    public class ReferenceCore : IEntityCore
    {
        private string _logicalName;
        private string _name;
        private Guid _id;

        public ReferenceCore(string logicalName, Guid id)
        {
            _logicalName = logicalName;
            _id = id;
        }

        public ReferenceCore(string logicalName, Guid id, string name)
        {
            _logicalName = logicalName;
            _id = id;
            _name = name;
        }

        [DataMember]
        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        [DataMember]
        public string LogicalName
        {
            get => _logicalName;
            set => _logicalName = value;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}