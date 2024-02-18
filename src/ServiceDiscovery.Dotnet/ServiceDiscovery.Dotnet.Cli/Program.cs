using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceDiscovery.Dotnet.Cli.Commands;
using ServiceDiscovery.Dotnet.Shared;
using ServiceDiscovery.Dotnet.Shared.Models;
using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;
using Spectre.Console;
using Spectre.Console.Cli;

internal class Program
{
    public static async Task Main(string[] args)
    {
        //var app = new CommandApp<FileSizeCommand>();
        //app.Run(args);
        var builder = Host.CreateApplicationBuilder(args);
        if (args?.Length > 0)
        {
            //builder.Services.GetServicesFromParameters(args, typeof(Program).Assembly?.GetName()?.Version?.ToString() ?? "Cannot obtain Version");
            //builder.Services.AddSingleton(new CliConfig { Args = args });


            var app = new CommandApp<FileSizeCommand>();
            app.Run(args);
        }
        else
        {
            builder.Services.AddSingleton(new RedisConnectionMultiplexer());
            builder.Services.AddSingleton(new RabbitConnection());
            var cliConfig = new CliConfig { Args = args };
            builder.Services.AddSingleton(cliConfig);
            builder.Services.AddSingleton(new CliAppConfig(new RedisConnectionMultiplexer(), new RabbitConnection(), cliConfig));
            builder.Services.AddSingleton<CliFlow>();
            var host = builder.Build();
            var cliFlow = host.Services.GetRequiredService<CliFlow>();
            await cliFlow.RunAsync();
        }
    }
}
