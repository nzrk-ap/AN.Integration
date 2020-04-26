using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AN.Integration._1C.Models
{
    public interface IOneCData
    {
        
        [JsonProperty(Required = Required.Always)]
        public string Code { get; set; }
    }
}