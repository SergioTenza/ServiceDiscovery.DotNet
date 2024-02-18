using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ServiceDiscovery.Dotnet.Cli.Commands
{
    internal sealed class BaseCommand : Command<BaseCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Path to search. Defaults to current directory.")]
            [CommandArgument(0, "[searchPath]")]
            public string? SearchPath { get; init; }

            [CommandOption("-p|--pattern")]
            public string? SearchPattern { get; init; }

            [CommandOption("-f|--file")]
            public string? FileName { get; init; }           
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var searchOptions = new EnumerationOptions
            {
                AttributesToSkip = FileAttributes.Hidden | FileAttributes.System
            };
            
            var searchPattern = settings.SearchPattern ?? "*.*";
            var searchPath = settings.SearchPath ?? Directory.GetCurrentDirectory();
            var files = new DirectoryInfo(searchPath)
                .GetFiles(searchPattern, searchOptions);
            if (files.Length == 0)
            {
                AnsiConsole.MarkupLine("No files found.");
                return 1;
            }            
            AnsiConsole.MarkupLine($"Found [green]{files.FirstOrDefault()!.Name}[/] files in [green]{searchPath}[/]");
            return 0;
        }
    }
}
