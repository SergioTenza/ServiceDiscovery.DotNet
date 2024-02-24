using ServiceDiscovery.Dotnet.Shared.Models;
using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliInteractiveFlow
{
    private readonly CliAppConfig _cliAppConfig;

    public CliInteractiveFlow(CliAppConfig cliAppConfig)
    {
        _cliAppConfig = cliAppConfig;
    }

    public async Task RunAsync()
    {       

        var communication = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
        .Title("Which [green]communication pattern[/] would you use?")
        .PageSize(3)
        .AddChoices(new[] {
                 "[red]Redis[/]", "[orange3]RabbitMQ[/]", "[green]Rest[/]"
        }));
        (string connectionString, string text) connection = communication switch
        {
            "[red]Redis[/]" => (AnsiConsole.Ask<string>("What's your [red]Redis[/] connectionstring?"), "Redis"),
            "[orange3]RabbitMQ[/]" => (AnsiConsole.Ask<string>("What's your [orange3]RabbitMQ[/] connectionstring?"), "RabbitMQ"),
            "[green]Rest[/]" => (AnsiConsole.Ask<string>("What's your [green]Rest[/] connectionstring?"), "Rest"),
            _ => (string.Empty, string.Empty)
        };

            
        var layout = new Layout("Commands");
        layout["Commands"]
            .SplitColumns(
                new Layout("Left")
                    .SplitRows(
                    new Layout("Options"))
                //new Layout("Right")
                //    .SplitRows(
                //        new Layout("Rest"),
                //        new Layout("Redis"),
                //        new Layout("RabbitMQ"))
                );
        // Render the layout
        AnsiConsole.Live(layout)
            .Start(ctx =>
            {

                ctx.Refresh();        
            });
        

        var newJson = JsonSerializer.Serialize(new
        {
            Communication = connection.connectionString,
        });
        var json = new JsonText(newJson);
        var panelJson = new Panel(json)
               .Header("Communication pattern")
               .Collapse()
               .RoundedBorder()
               .BorderColor(Color.Yellow);
        // Update the left column
        layout["Options"].Update(panelJson);

        
        

        AnsiConsole.Markup($"Selected {communication} with ConnectionString: [underline Blue]{connection.connectionString}[/] ");

        AnsiConsole.WriteLine($"Stablishing connection please wait.");

        switch (connection.text)
        {
            case "Redis":
                try
                {
                    var isConnected = await _cliAppConfig.ConnectToRedis(connection.connectionString);
                    AnsiConsole.MarkupLineInterpolated($"""[red]Redis[/] connection established: [{(isConnected ? "green" : "red")}]{isConnected}[/]""");
                }
                catch (Exception)
                {
                    AnsiConsole.MarkupLineInterpolated($"""[red]Redis[/] connection [underline Red]not established[/]: [red]false[/]""");
                    //AnsiConsole.WriteException(ex);
                }

                break;
            case "RabbitMQ":
                break;
            case "Rest":
                break;
            default:
                AnsiConsole.WriteLine($"Connection unknown. Closing system.");
                Environment.Exit(1);
                break;

        }
    }

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
