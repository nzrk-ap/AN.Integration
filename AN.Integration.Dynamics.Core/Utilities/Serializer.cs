using System.IO;
using System.Runtime.Serialization.Json;

namespace AN.Integration.Dynamics.Core.Utilities
{
    public static class Serializer
    {
        public static T Deserialize<T>(byte[] data)
        {
            T deserializedObject;
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(data))
            {
                deserializedObject = (T)serializer.ReadObject(stream);
            }

            return deserializedObject;
        }
    }
}
