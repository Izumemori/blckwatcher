using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DapiPlaysBlckStatus.Entities.Extensions
{
    internal static class StreamExtensions
    {
        public static Task WriteAsync(this MemoryStream stream, int value)
        {
            var buffer = new List<byte>();

            while (value > 127)
            {
                buffer.Add((byte) (value & 127 | 128));
                value = (int) ((uint) value) >> 7;
            }
            buffer.Add((byte) value);

            return stream.WriteAsync(buffer.ToArray(), 0, buffer.Count);
        }

        public static Task WriteAsync(this MemoryStream stream, short value)
        {
            var bytes = BitConverter.GetBytes(value);

            return stream.WriteAsync(bytes, 0, bytes.Length);            
        }

        public static async Task WriteAsync(this MemoryStream stream, string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            await stream.WriteAsync(bytes.Length);
            
            await stream.WriteAsync(bytes, 0, bytes.Length);            
        }

        public static Task WriteAsync(this MemoryStream stream, byte b)
            => stream.WriteAsync(new [] {b}, 0, 1);

        public static void Reset(this MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
        }
    }
}