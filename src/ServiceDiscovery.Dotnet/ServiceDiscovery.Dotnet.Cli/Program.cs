using Spectre.Console;
using StackExchange.Redis;

internal class Program
{

    static ConnectionMultiplexer RedisConnection;
    public static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            //TODO: Pure CLI flow

            RedisConnection = ConnectionMultiplexer.Connect(args[5]);
        }
        else
        {
            AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
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

            AnsiConsole.Markup($"Selected {communication} with ConnectionString: [underline Blue]{connection.connectionString}[/] ");

            AnsiConsole.WriteLine($"Stablishing connection please wait.");

            switch (connection.text)
            {
                case "Redis":
                    try
                    {
                        RedisConnection = await ConnectionMultiplexer.ConnectAsync(connection.connectionString);
                        AnsiConsole.WriteLine(RedisConnection.IsConnected);
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
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


    }
}