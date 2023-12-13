using Spectre.Console;
using StackExchange.Redis;

internal class Program
{

    static ConnectionMultiplexer RedisConnection;
    public static void Main(string[] args)
    {
        if(args.Length > 0)
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
            (string connectionString,string text) connection = communication switch 
            {
                "[red]Redis[/]" => (AnsiConsole.Ask<string>("What's your [red]Redis[/] connectionstring?"),"Redis"),
                "[orange3]RabbitMQ[/]" => (AnsiConsole.Ask<string>("What's your [orange3]RabbitMQ[/] connectionstring?"),"RabbitMQ"),
                "[green]Rest[/]" => (AnsiConsole.Ask<string>("What's your [green]Rest[/] connectionstring?"),"Rest"),
                _ => (string.Empty,string.Empty)
            };

            AnsiConsole.Markup($"Selected {communication} with ConnectionString: [underline Blue]{connection.connectionString}[/] ");
        }

        
    }
}