using Microsoft.Extensions.DependencyInjection;
using Mono.Options;
using ServiceDiscovery.Dotnet.Shared.Models;
using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;
using Spectre.Console;

namespace ServiceDiscovery.Dotnet.Shared.Hosting
{
    public static class ParametersServicesProvider
    {
        public static IServiceCollection GetServicesFromParameters(this IServiceCollection services, string[] args, string version)
        {
            var shouldShowHelp = false;
            var shouldShowVersion = false;
            var cliAppConfig = new CliAppConfig(new RedisConnectionMultiplexer(), new RabbitConnection(), new CliConfig { Args = args });
            var options = new OptionSet
            {
                { "v|version=", "Shows version Info", v => shouldShowVersion = v != null },
                { "c|config=", "config file path", n => cliAppConfig = cliAppConfig.AddConfigPath(n) },
                { "h|help", "shows accepted commands", h => shouldShowHelp = h != null }
            };
            List<string> extra;
            try
            {   
                extra = options.Parse(args);
                if (!shouldShowHelp)
                {
                    services.AddSingleton(cliAppConfig);
                }
            }
            catch (OptionException e)
            {
                // output some error message
                AnsiConsole.Write("ServiceDiscovery.Dotnet.Cli: ");
                AnsiConsole.WriteLine(e.Message);
                AnsiConsole.WriteLine("Try `ServiceDiscovery.Dotnet.Cli --help | --h' for more information.");
                return services;
            }
            if (shouldShowHelp)
            {
                ShowHelpAndExit(options);
            }
            if (shouldShowVersion)
            {
                ShowVersionAndExit(version);
            }
            return services;
        }
        private static void ShowHelpAndExit(OptionSet options)
        {
            AnsiConsole.MarkupLine("[blue]ServiceDiscovery.Dotnet.Cli[/] ");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[orange4_1]received help command.[/]");
            AnsiConsole.MarkupLine("[orange4_1]Showing help commands.[/]");
            AnsiConsole.WriteLine();
            foreach (var option in options)
            {
                AnsiConsole.MarkupLine($"[green]{option.Prototype}[/]: [yellow]{option.Description}[/]");
            }
            Environment.Exit(0);
        }
        private static void ShowVersionAndExit(string version)
        {   
            AnsiConsole.MarkupLineInterpolated($"[blue]ServiceDiscovery.Dotnet.Cli[/] [green]V.{version}[/]");
            Environment.Exit(0);
        }
        private static CliAppConfig AddConfigPath(this CliAppConfig appConfig, string configPath) =>
            appConfig with { ConfigPathName = configPath };
    }
}
