using System;
using System.Runtime.Serialization;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
    [DataContract]
    public sealed class EntityCore : IEntityCore
    {
        private string _logicalName;
        private Guid _id;
        private AttributeCollectionCore _attributes;

        public EntityCore(string logicalName)
        {
            _logicalName = logicalName;
        }

        public EntityCore(string logicalName, Guid id)
        {
            _logicalName = logicalName;
            _id = id;
        }

        public EntityCore(string entityName, AttributeCollectionCore attributes)
        {
            _logicalName = entityName;
            _attributes = attributes;
        }

        [DataMember]
        public string LogicalName
        {
            get => _logicalName;
            set => _logicalName = value;
        }

        [DataMember]
        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        [DataMember]
        public AttributeCollectionCore Attributes
        {
            get => _attributes ??= new AttributeCollectionCore();
            set => _attributes = value;
        }

        public object this[string attributeName]
        {
            get => Attributes[attributeName];
            set => Attributes[attributeName] = value;
        }

        public bool Contains(string attributeName) =>
            Attributes.Contains(attributeName);

        public ReferenceCore ToEntityReference() =>
            new ReferenceCore(LogicalName, Id);

        public T GetAttributeValue<T>(string attributeLogicalName)
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