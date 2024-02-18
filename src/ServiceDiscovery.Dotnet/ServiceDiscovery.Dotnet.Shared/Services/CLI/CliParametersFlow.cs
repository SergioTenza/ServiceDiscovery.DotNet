using Spectre.Console;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliParametersFlow
{
    public static async Task Greet(CliConfig config)
    {
        var message = $"{config.Args.Length} Parameters received initializing operations...";
            AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
            AnsiConsole.MarkupLineInterpolated($"[underline Blue]{message}[/]\r\n");                
    }
}
