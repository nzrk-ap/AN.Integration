using System;
using System.Runtime.Serialization;

namespace AN.Integration.Dynamics.DynamicsContextTypes
{
    public class DynamicsEntity : IExtensibleDataObject
    {
        private string _logicalName;
        private Guid _id;
        private DynamicsAttributeCollection _attributes;

        public DynamicsEntity(string logicalName)
        {
            _logicalName = logicalName;
        }

        public DynamicsEntity(string logicalName, Guid id)
        {
            _logicalName = logicalName;
            _id = id;
        }

        public DynamicsEntity(string entityName, DynamicsAttributeCollection attributes)
        {
            _logicalName = entityName;
            _attributes = attributes;
        }

        public string LogicalName
        {
            get => _logicalName;
            set => _logicalName = value;
        }

        public virtual Guid Id
        {
            get => _id;
            set => _id = value;
        }

        public DynamicsAttributeCollection Attributes
        {
            get => _attributes ?? (_attributes = new DynamicsAttributeCollection());
            set => _attributes = value;
        }

        public object this[string attributeName]
        {
            get => Attributes[attributeName];
            set => Attributes[attributeName] = value;
        }

        public ExtensionDataObject ExtensionData { get; set; }

        public bool Contains(string attributeName) =>
            Attributes.Contains(attributeName);

        public DynamicsReference ToEntityReference() =>
            new DynamicsReference(LogicalName, Id);

        public virtual T GetAttributeValue<T>(string attributeLogicalName)
        {
            var attributeValue = GetAttributeValue(attributeLogicalName);
            return attributeValue == null ? default : (T)attributeValue;
        }

        private object GetAttributeValue(string attributeLogicalName)
        {
            if (string.IsNullOrWhiteSpace(attributeLogicalName))
                throw new ArgumentNullException(nameof(attributeLogicalName));
            return !Contains(attributeLogicalName) ? null : this[attributeLogicalName];
        }
    }
}