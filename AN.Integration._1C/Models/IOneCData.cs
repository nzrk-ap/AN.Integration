using Newtonsoft.Json;

namespace AN.Integration._1C.Models
{
    public interface IOneCData
    {
        [JsonProperty(Required = Required.Always)]
        string Code { get; set; }
    }
}