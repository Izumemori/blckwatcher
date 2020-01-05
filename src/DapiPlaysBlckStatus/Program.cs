﻿using System;
using System.Threading.Tasks;
using DapiPlaysBlckStatus.Entities;
using DapiPlaysBlckStatus.Services;
using Disqord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace DapiPlaysBlckStatus
{
    public static class Program
    {
        public static Task Main(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configBuilder =>
            {
                configBuilder.AddEnvironmentVariables("BLCK_");
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddNLog();
            })
            .ConfigureServices((hostContext, serviceCollection) =>
            {
                serviceCollection.Configure<BotConfig>(hostContext.Configuration.GetSection("BOT"));
                serviceCollection.Configure<BlckConfig>(hostContext.Configuration.GetSection("BLCK"));
                serviceCollection.Configure<Qmmands.CommandServiceConfiguration>(conf =>
                {
                    conf.DefaultRunMode = Qmmands.RunMode.Parallel;
                });
                serviceCollection.AddSingleton<Qmmands.CommandService>(commandServiceBuilder => new Qmmands.CommandService(commandServiceBuilder.GetRequiredService<IOptions<Qmmands.CommandServiceConfiguration>>().Value));
                serviceCollection.AddSingleton<DiscordClient>(clientBuilder =>
                    new DiscordClient(TokenType.Bot,
                        clientBuilder.GetRequiredService<IOptions<BotConfig>>().Value.Token));
                serviceCollection.AddHostedService<EventService>();
                serviceCollection.AddHostedService<BotService>();
                serviceCollection.AddHostedService<BlckService>();
            })
            .RunConsoleAsync();
    }
}