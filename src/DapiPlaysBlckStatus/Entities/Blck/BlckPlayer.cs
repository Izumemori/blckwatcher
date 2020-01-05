using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckPlayer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}