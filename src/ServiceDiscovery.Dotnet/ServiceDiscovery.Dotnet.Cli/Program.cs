using Spectre.Console;

internal class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
        var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
        AnsiConsole.WriteLine($"Hi {name} welcome to 'ServiceDiscovery.Dotnet' ");
    }
}