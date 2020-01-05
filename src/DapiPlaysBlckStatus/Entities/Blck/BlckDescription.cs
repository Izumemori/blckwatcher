using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckDescription
    {
        [JsonProperty("text")]
        public string Motd { get; set; }
    }
}