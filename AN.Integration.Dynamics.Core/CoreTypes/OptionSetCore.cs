using System.Runtime.Serialization;

namespace AN.Integration.DynamicsCore.CoreTypes
{
    [DataContract]
    public class OptionSetCore
    {
        private int _value;

        public OptionSetCore()
        {
        }

        public OptionSetCore(int value)
        {
            _value = value;
        }

        [DataMember]
        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OptionSetCore optionSetCoreValue))
                return false;
            return this == optionSetCoreValue || _value.Equals(optionSetCoreValue._value);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}