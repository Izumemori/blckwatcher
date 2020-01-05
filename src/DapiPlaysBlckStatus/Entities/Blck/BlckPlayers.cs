#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override bool Equals(object? obj)
        {
            if (!(obj is BlckPlayers other)) return false;

            return Equals(other);
        }

        protected bool Equals(BlckPlayers other)
        {
            if (!this.MaxPlayers.Equals(other.MaxPlayers)) return false;
            
            if (!this.PlayerCount.Equals(other.PlayerCount)) return false;
            
            if (this.Players == null && other.Players != null
                || this.Players != null && other.Players == null) return false;
            
            if (this.Players == null && other.Players == null) return true;


            var compare = other.Players?.Select(x => x.Id).OrderBy(x => x) ??
                          (IEnumerable<string>) new string[0];
            if (!(this.Players?.Select(x => x.Id).OrderBy(x => x).SequenceEqual(compare)) ?? false) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MaxPlayers, PlayerCount, Players);
        }
    }
}