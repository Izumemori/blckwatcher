using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckVersion
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("protocol")]
        public int ProtocolVersion { get; set; }
    }
}