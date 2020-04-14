using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.Utilities
{
    public static class ContextSerializer
    {
        public static string ToJson(DynamicsContextCore contextCore)
        {
            var knownTypes = new List<Type>
            {
                typeof(EntityCore),
                typeof(ReferenceCore),
            };

            var serializerSettings = new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            };

            var serializer = new DataContractJsonSerializer(typeof(DynamicsContextCore), serializerSettings);
            string json;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, contextCore);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            return json;
        }

        public static DynamicsContextCore ToContext(byte[] binaryContext)
        {
            var knownTypes = new List<Type>()
            {
                typeof(EntityCore),
                typeof(ReferenceCore),
            };

            var serializerSettings = new DataContractJsonSerializerSettings
            {
                KnownTypes = knownTypes
            };

            DynamicsContextCore deserializedObject;
            var serializer = new DataContractJsonSerializer(typeof(DynamicsContextCore), serializerSettings);
            using (var stream = new MemoryStream(binaryContext))
            {
                deserializedObject = (DynamicsContextCore) serializer.ReadObject(stream);
            }

            return deserializedObject;
        }
    }
}