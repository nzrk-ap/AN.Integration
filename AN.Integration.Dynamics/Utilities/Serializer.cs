using System.IO;
using System.Runtime.Serialization.Json;
using AN.Integration.Models.Dynamics;

namespace AN.Integration.Dynamics.Utilities
{
    public static class Serializer
    {
        public static T Deserialize<T>(byte[] data)
        {
            T deserializedObject;
            var serializer = new DataContractJsonSerializer(typeof(DynamicsContext));
            using (var stream = new MemoryStream(data))
            {
                deserializedObject = (T)serializer.ReadObject(stream);
            }

            return deserializedObject;
        }
    }
}
