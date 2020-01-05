using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using DapiPlaysBlckStatus.Entities;
using DapiPlaysBlckStatus.Entities.Blck;
using Disqord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DapiPlaysBlckStatus.Services
{
    public class BlckService : BackgroundService
    {
        private readonly ILogger<BlckService> _logger;
        private readonly BlckConfig _config;
        private readonly DiscordClient _client;

        public BlckService(ILogger<BlckService> logger, IOptions<BlckConfig> options, DiscordClient client)
            => (this._logger, this._config, this._client) = (logger, options.Value, client);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ITextChannel channel = null;
            IUserMessage lastMsg = null;
            do
            {
                await Task.Delay(5*60_000);
                try
                {
                    if (channel is null)
                        channel = (await this._client.GetChannelAsync((ulong)this._config.Channel)) as ITextChannel;

                    if (lastMsg is null)
                        lastMsg = (await channel.GetMessagesAsync()).FirstOrDefault(x =>
                            x.Author.Id == this._client.CurrentUser.Id) as IUserMessage;
                    
                    BlckStatus? status = null;
                    await using (var mcClient = new MCClient(this._config.Server, this._config.Port))
                    {
                        status = await mcClient.GetStatusAsync();
                    }


                    var embed = new LocalEmbedBuilder()
                        .WithAuthor(builder => builder.WithName("Blck Status"))
                        .WithColor(Color.Coral)
                        .WithDescription(
                            status is null ? ":x: Server is offline" : ":white_check_mark: Server is online" );

                    if (status != null)
                    {
                        embed.AddField(builder => builder.WithName("Motd").WithValue(status.Description.Motd));
                        
                        if (status.Players.Players != null)
                        {
                            embed.AddField(builder =>
                                builder.WithName("Players")
                                    .WithValue(string.Join("\n", status.Players.Players.OrderBy(x => x.Name).Select(x => x.Name))));
                        }
                        
                        embed.AddField(builder => builder.WithBlankName().WithValue($"{status.Players.PlayerCount}/{status.Players.MaxPlayers}").WithIsInline(false));
                    }

                    if (lastMsg is null)
                        lastMsg = await channel.SendMessageAsync(embed: embed.Build());
                    else
                        await lastMsg.ModifyAsync(x => x.Embed = embed.Build());
                }
                catch{}
            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}