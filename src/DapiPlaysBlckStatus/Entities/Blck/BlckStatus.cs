using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckStatus
    {
        [JsonProperty("description")]
        public BlckDescription Description { get; set; }
        
        [JsonProperty("players")]
        public BlckPlayers Players { get; set; }
        
        [JsonProperty("version")]
        public BlckVersion Version { get; set; }
    }
}