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
        }
        else
        {
            AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
            var communication = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
            .Title("Which [green]communication pattern[/] would you use?")
            .PageSize(3)        
            .AddChoices(new[] {
                "Rest", "RabbitMQ", "Redis"
            }));
            var connection = communication switch 
            {
                "Redis" => AnsiConsole.Ask<string>("What's your [green]Redis[/] connectionstring?"),
                "RabbitMQ" => AnsiConsole.Ask<string>("What's your [green]RabbitMQ[/] connectionstring?"),
                "Rest" => AnsiConsole.Ask<string>("What's your [green]Rest[/] connectionstring?"),
                _ => string.Empty
            };

            AnsiConsole.WriteLine($"Selected {communication} with ConnectionString: '{connection}' ");
        }

        
    }
}