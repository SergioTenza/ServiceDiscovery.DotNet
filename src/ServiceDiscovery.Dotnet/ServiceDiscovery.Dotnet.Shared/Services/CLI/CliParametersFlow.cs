using ServiceDiscovery.Dotnet.Shared.Models;
using Spectre.Console;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliParametersFlow
{
    public static async Task Greet(CliAppConfig config)
    {   
        AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
        AnsiConsole.MarkupLineInterpolated($"[underline Blue]{config.Args.Length } Parameters received initializing operations...[/]\r\n");
        await Task.CompletedTask;
    }
}
