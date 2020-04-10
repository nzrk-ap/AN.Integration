using System;
using System.Runtime.Serialization;

namespace AN.Integration.Dynamics.DynamicsContextTypes
{
   public class DynamicsReference: IExtensibleDataObject
    {
        private string _logicalName;
        private string _name;
        private Guid _id;

        public DynamicsReference(string logicalName, Guid id)
        {
            _logicalName = logicalName;
            _id = id;
        }

        public DynamicsReference(string logicalName, Guid id, string name)
        {
            _logicalName = logicalName;
            _id = id;
            _name = name;
        }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
