using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceDiscovery.Dotnet.Cli.Commands;
using ServiceDiscovery.Dotnet.Shared;
using ServiceDiscovery.Dotnet.Shared.Models;
using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        if (args?.Length > 0)
        {
            var app = new CommandApp<BaseCommand>();
            app.Configure(config =>
            {
                config.AddCommand<RouteCommand>("route");                
            });            
            await app.RunAsync(args).ConfigureAwait(true);
        }
        else
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSingleton(new RedisConnectionMultiplexer());
            builder.Services.AddSingleton(new RabbitConnection());
            builder.Services.AddSingleton(services =>
            {
                return new CliAppConfig(services.GetRequiredService<RedisConnectionMultiplexer>());
            });
            builder.Services.AddSingleton<CliInteractiveFlow>();
            var host = builder.Build();
            var cliFlow = host.Services.GetRequiredService<CliInteractiveFlow>();
            await cliFlow.RunAsync().ConfigureAwait(true);
        }
    }
}
