#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System;
using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckVersion
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("protocol")]
        public int ProtocolVersion { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is BlckVersion other)) return false;

            return Equals(other);
        }

        protected bool Equals(BlckVersion other)
        {
            if (!this.Name.Equals(other.Name)) return false;

            if (!this.ProtocolVersion.Equals(other.ProtocolVersion)) return false;
            
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ProtocolVersion);
        }
    }
}