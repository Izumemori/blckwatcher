using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using DapiPlaysBlckStatus.Entities.Blck;
using DapiPlaysBlckStatus.Entities.Extensions;
using Newtonsoft.Json;

namespace DapiPlaysBlckStatus.Entities
{
    public class MCClient : IAsyncDisposable
    {
        private readonly string _host;
        private readonly short _port;
        private readonly TcpClient _client;
        private bool isDisposed = false;
        private Stream stream => this._client?.GetStream();
        private MemoryStream _buffer;

        public MCClient(string host, short port)
        {
            this._host = host;
            this._port = port;
            this._client = new TcpClient();
            this._buffer = new MemoryStream();
        }

        private async Task ConnectAsync()
        {
            if (this.isDisposed)
                throw new Exception($"{nameof(MCClient)} cannot be used after being disposed");

            if (this._client.Connected)
                return;

            await this._client.ConnectAsync(this._host, this._port);

            await SendHandshakeAsync();
        }

        private async Task SendHandshakeAsync()
        {
            await this._buffer.WriteAsync(575);
            await this._buffer.WriteAsync(this._host);
            await this._buffer.WriteAsync(this._port);
            await this._buffer.WriteAsync(1);
            await SendAsync(0);
        }

        public async Task<BlckStatus?> GetStatusAsync()
        {
            try
            {
                await ConnectAsync();
                await SendAsync(0);

                var buffer = new byte[Int16.MaxValue];
                await this.stream.ReadAsync(buffer, 0, buffer.Length);

                await SendAsync(1);

                var offset = 0;

                var length = buffer.ReadVarInt(ref offset);
                var packet = buffer.ReadVarInt(ref offset);
                var jsonLength = buffer.ReadVarInt(ref offset);

                var json = buffer.ReadString(jsonLength, ref offset);

                Console.WriteLine(json);
                
                return JsonConvert.DeserializeObject<BlckStatus>(json);
            }
            catch
            {
                return null;
            }
        }

        private async Task SendAsync(int id)
        {
            var buffer = this._buffer.ToArray();
            this._buffer.Reset();

            var add = 0;
            var packetData = new[] {(byte) 0x00};
            if (id >= 0)
            {
                await this._buffer.WriteAsync(id);
                packetData = _buffer.ToArray();
                add += packetData.Length;
                this._buffer.Reset();
            }

            await this._buffer.WriteAsync(buffer.Length + add);
            var bufferLength = _buffer.ToArray();
            this._buffer.Reset();
            
            this.stream.Write(bufferLength, 0, bufferLength.Length);
            this.stream.Write(packetData, 0, packetData.Length);
            this.stream.Write(buffer, 0, buffer.Length);
        }

        public ValueTask DisposeAsync()
        {
            this.isDisposed = true;
            if (this._client.Connected)
                this._client.Close();

            this._client.Dispose();

            return new ValueTask();
        }
    }
}
