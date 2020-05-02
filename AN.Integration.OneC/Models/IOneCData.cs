using Newtonsoft.Json;

namespace AN.Integration.OneC.Models
{
    public interface IOneCData
    {
        [JsonProperty(Required = Required.Always)]
        string Code { get; set; }
    }
}