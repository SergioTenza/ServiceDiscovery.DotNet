using Microsoft.AspNetCore.Routing.Constraints;
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

            [Description("Pattern for extension search. Defaults to *.*")]
            [CommandOption("-p|--pattern")]
            public string? SearchPattern { get; init; }

            [Description("Filename to search.")]
            [CommandOption("-f|--file")]
            public string? FileName { get; init; }

            public override ValidationResult Validate()
            {
                return FileName?.Length <= 0
                    ? ValidationResult.Error("FileName must be introduced")
                    : ValidationResult.Success();
            }
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
                AnsiConsole.MarkupLine("[red]No files found.[/]");
                return 1;
            }
            if (!string.IsNullOrEmpty(settings.FileName))
            {
                var coincidence = files.Where(f => f.Name == settings.FileName).FirstOrDefault();
                if (coincidence is null) 
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]file [darkgoldenrod]{settings.FileName}[/] was not found on supplied path [blue]{searchPath}[/][/]");
                    return 1;
                }
                AnsiConsole.MarkupLineInterpolated($"Found [green]{coincidence.Name}[/] in [blue]{searchPath}[/]");
                return 0;
            }
            AnsiConsole.MarkupLineInterpolated($"Found [darkgoldenrod]{files.Length}[/] files");
            Array.ForEach(files, file =>
            {
                AnsiConsole.MarkupLineInterpolated($"[green]{file.Name}[/]");                
            });
            AnsiConsole.MarkupLineInterpolated($"in [blue]{searchPath}[/]");
            AnsiConsole.WriteLine();
            var communication = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Which [green]file[/] do you want to use?")
                .PageSize(3)
                .AddChoices(files.Select(f => $"[white]{f.Name}[/]" )));
            return 0;
        }
    }
}
