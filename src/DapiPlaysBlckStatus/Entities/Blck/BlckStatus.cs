#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System;
using System.Linq;
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

        public override bool Equals(object? obj)
        {
            if (!(obj is BlckStatus other)) return false;

            return Equals(other);
        }

        protected bool Equals(BlckStatus other)
        {
            if (!this.Description.Equals(other.Description)) return false;

            if (!this.Version.Equals(other.Version)) return false;
            
            if (!this.Players.Equals(other.Players)) return false;
            
            return true;        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, Players, Version);
        }

        public override string ToString()
            => $"Motd: {Description.Motd}\n" +
                $"Players: {Players.PlayerCount}/{Players.MaxPlayers}" +
                (this.Players.Players is null ? string.Empty : $" ({string.Join(", ", Players.Players.Select(x => x.Name))})") + "\n" +
                $"Version: {this.Version.Name} ({this.Version.ProtocolVersion})";
    }
}