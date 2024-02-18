using ServiceDiscovery.Dotnet.Shared.Models;
using Spectre.Console;

namespace ServiceDiscovery.Dotnet.Shared;

public class CliFlow
{
    private readonly CliAppConfig _config;

    public CliFlow(CliAppConfig config)
    {
        _config = config;
    }

    public async Task RunAsync()   
    {
        if(_config.Args is { } and {Length: > 0 })
        {
           await CliParametersFlow.Greet(_config);
        }
        else if(_config.Args is { } and {Length: <= 0 })
        {
            await CliInteractiveFlow.Greet(_config);
        }
        else
        {
            var message = "Not supported initial parameters";
            AnsiConsole.Markup("[underline Green]ServiceDiscovery.Dotnet[/]\r\n");
            AnsiConsole.MarkupLineInterpolated($"[underline Blue]{message}[/]\r\n");
        }
        await Task.CompletedTask;
    }
}