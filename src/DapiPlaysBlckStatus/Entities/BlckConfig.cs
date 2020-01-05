#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System;

namespace DapiPlaysBlckStatus.Entities
{
    public class BlckConfig
    {
        public ulong Channel { get; set; }
        public string Server { get; set; }
        public short Port { get; set; }
    }
}