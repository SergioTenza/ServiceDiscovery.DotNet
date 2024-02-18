using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceDiscovery.Dotnet.Shared;
using Spectre.Console;
using StackExchange.Redis;

internal class Program
{

    static ConnectionMultiplexer RedisConnection;
    public static async Task Main(string[] args)
    {

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddSingleton(new CliConfig{ Args = args?.Length > 0 ? args : []});
        builder.Services.AddSingleton<CliFlow>();
        var host = builder.Build();
        var cliFlow = host.Services.GetRequiredService<CliFlow>();
        await cliFlow.RunAsync();
        //await host.RunAsync();
        
        // if (args.Length > 0)
        // {
        //     //TODO: Pure CLI flow

        //     RedisConnection = ConnectionMultiplexer.Connect(args[5]);
        // }
        // else
        // {
        //     AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
        //     var communication = AnsiConsole.Prompt(
        //         new SelectionPrompt<string>()
        //     .Title("Which [green]communication pattern[/] would you use?")
        //     .PageSize(3)
        //     .AddChoices(new[] {
        //         "[red]Redis[/]", "[orange3]RabbitMQ[/]", "[green]Rest[/]"
        //     }));
        //     (string connectionString, string text) connection = communication switch
        //     {
        //         "[red]Redis[/]" => (AnsiConsole.Ask<string>("What's your [red]Redis[/] connectionstring?"), "Redis"),
        //         "[orange3]RabbitMQ[/]" => (AnsiConsole.Ask<string>("What's your [orange3]RabbitMQ[/] connectionstring?"), "RabbitMQ"),
        //         "[green]Rest[/]" => (AnsiConsole.Ask<string>("What's your [green]Rest[/] connectionstring?"), "Rest"),
        //         _ => (string.Empty, string.Empty)
        //     };

        //     AnsiConsole.Markup($"Selected {communication} with ConnectionString: [underline Blue]{connection.connectionString}[/] ");

        //     AnsiConsole.WriteLine($"Stablishing connection please wait.");

        //     switch (connection.text)
        //     {
        //         case "Redis":
        //             try
        //             {
        //                 RedisConnection = await ConnectionMultiplexer.ConnectAsync(connection.connectionString);
        //                 AnsiConsole.MarkupLineInterpolated($"""[red]Redis[/] connection established: [{(RedisConnection.IsConnected ? "green": "red")}]{RedisConnection.IsConnected}[/]""");
        //             }
        //             catch (Exception)
        //             {
        //                 AnsiConsole.MarkupLineInterpolated($"""[red]Redis[/] connection [underline Red]not established[/]: [red]false[/]""");
        //                 //AnsiConsole.WriteException(ex);
        //             }

        //             break;
        //         case "RabbitMQ":
        //             break;
        //         case "Rest":
        //             break;
        //         default:
        //             AnsiConsole.WriteLine($"Connection unknown. Closing system.");
        //             Environment.Exit(1);
        //             break;

        //     }

        // }


    }
}
