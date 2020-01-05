#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities.Blck
{
    public class BlckDescription
    {
        [JsonProperty("text")] 
        public string Motd { get; set; }

        public override bool Equals(object? obj)
        {
            if (!(obj is BlckDescription other)) return false;

            return Equals(other);
        }

        protected bool Equals(BlckDescription other)
        {
            return this.Motd.Equals(other.Motd);
        }

        public override int GetHashCode()
        {
            return Motd.GetHashCode();
        }
    }
}