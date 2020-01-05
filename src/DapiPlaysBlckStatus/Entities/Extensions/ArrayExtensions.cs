using System;
using System.IO;
using System.Text;

namespace DapiPlaysBlckStatus.Entities.Extensions
{
    public static class ArrayExtensions
    {
        public static byte ReadByte(this byte[] buffer, ref int offset)
        {
            var b = buffer[offset];
            offset += 1;
            return b;
        }

        internal static byte[] Read(this byte[] buffer, int length, ref int offset)
        {
            var data = buffer.AsSpan().Slice(offset, length);
            offset += length;
            return data.ToArray();
        }

        internal static int ReadVarInt(this byte[] buffer, ref int offset)
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = buffer.ReadByte(ref offset)) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++*7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            return value | ((b & 0x7F) << (size*7));
        }

        internal static string ReadString(this byte[] buffer, int length, ref int offset)
        {
            var data = Read(buffer, length, ref offset);
            return Encoding.UTF8.GetString(data);
        }
    }
}