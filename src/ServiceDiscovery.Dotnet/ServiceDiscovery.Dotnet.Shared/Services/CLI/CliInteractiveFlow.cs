using ServiceDiscovery.Dotnet.Shared.Models;
using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliInteractiveFlow
{
    public static async Task Greet(CliAppConfig cliAppConfig)
    {

        //AnsiConsole.MarkupLine("[underline Green]ServiceDiscovery.Dotnet[/]");
        //AnsiConsole.MarkupLine($"[underline Blue]No paramaters received initializing interactive operations...[/]");

        // Create the layout
        var json = new JsonText(
        """
        { 
            "config" : null
        }
        """);
        var panelJson = new Panel(json)
               .Header("Communication pattern")
               .Collapse()
               .RoundedBorder()
               .BorderColor(Color.Yellow);
        var layout = new Layout("Commands");
        layout["Commands"]
            .SplitColumns(
                new Layout("Left")
                    .SplitRows(
                    new Layout("Options")),
                new Layout("Right")
                    .SplitRows(
                        new Layout("Rest"),
                        new Layout("Redis"),
                        new Layout("RabbitMQ")));

        // Update the left column
        layout["Options"].Update(panelJson);

        // Render the layout
        AnsiConsole.Write(layout);


        //var grid = new Grid();

        //// Add columns 
        //grid.AddColumn();

        //// Add header row 
        //grid.AddRow(new Text[]{
        //        new Text("Communication", new Style(Color.NavajoWhite1, Color.Black)).LeftJustified(),
        //        //new Text("Header 2", new Style(Color.Green, Color.Black)).Centered(),
        //        //new Text("Header 3", new Style(Color.Blue, Color.Black)).RightJustified()
        //    });

        // Add content row 
        //grid.AddRow(new Text[]{
        //        new Text("Row 1").LeftJustified(),
        //        new Text("Row 2").Centered(),
        //        new Text("Row 3").RightJustified()
        //    });

        // Write centered cell grid contents to Console
        //AnsiConsole.Write(grid);

        //AnsiConsole.Write(panelJson);


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


        var newJson = JsonSerializer.Serialize(new
        {
            Communication = connection.connectionString,
        });
        json = new JsonText(newJson);
        panelJson = new Panel(json)
               .Header("Communication pattern")
               .Collapse()
               .RoundedBorder()
               .BorderColor(Color.Yellow);
        // Update the left column
        layout["Options"].Update(panelJson);

        // Render the layout
        AnsiConsole.Write(layout);

        AnsiConsole.Markup($"Selected {communication} with ConnectionString: [underline Blue]{connection.connectionString}[/] ");

        AnsiConsole.WriteLine($"Stablishing connection please wait.");

        switch (connection.text)
        {
            case "Redis":
                try
                {
                    var isConnected = await cliAppConfig.ConnectToRedis(connection.connectionString);
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
