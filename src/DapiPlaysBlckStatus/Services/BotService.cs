using System.Threading;
using System.Threading.Tasks;
using Disqord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DapiPlaysBlckStatus.Services
{
    public class BotService : IHostedService
    {
        private readonly DiscordClient _client;
        private readonly ILogger<BotService> _logger;

        public BotService(ILogger<BotService> logger, DiscordClient client)
            => (this._logger, this._client) = (logger, client);
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var completionSource = new TaskCompletionSource<bool>();
            
            this._client.Ready += args =>
            {
                completionSource.SetResult(true);

                return Task.CompletedTask;
            };
            
            this._logger.LogInformation("Logging in...");
            
            _ = this._client.RunAsync();

            return completionSource.Task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => this._client.StopAsync();
    }
}