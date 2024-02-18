using System.Runtime.CompilerServices;
using Spectre.Console;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliInteractiveFlow
{
    public static async Task Greet()
    {
        var message = $"No paramaters received initializing interactive operations...";
        AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
        AnsiConsole.MarkupLineInterpolated($"[underline Blue]{message}[/]\r\n");    
    }
}
