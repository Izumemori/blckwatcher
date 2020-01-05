using System.Collections.Generic;
using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckPlayers
    {
        [JsonProperty("max")]
        public int MaxPlayers { get; set; }
        
        [JsonProperty("online")]
        public int PlayerCount { get; set; }
        
        [JsonProperty("sample")]
        public IEnumerable<BlckPlayer>? Players { get; set; }
    }
}