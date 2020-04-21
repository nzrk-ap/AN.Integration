using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace AN.Integration.Dynamics.Core.Utilities
{
    public static class DataSerializer
    {
        public static string ToJson<T>(T value)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using var ms = new MemoryStream();
            serializer.WriteObject(ms, value);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}