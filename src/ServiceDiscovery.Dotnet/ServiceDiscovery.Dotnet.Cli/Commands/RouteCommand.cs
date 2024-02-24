using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDiscovery.Dotnet.Cli.Commands
{
    public sealed class RouteCommand: Command<RouteCommand.Settings>
    {

        public sealed class Settings : CommandSettings
        {
            [Description("RouteId to add. Adds new route to api gateway.")]
            [CommandArgument(0, "[searchPath]")]
            public string? SearchPath { get; init; }

            [Description("Pattern for extension search. Defaults to *.*")]
            [CommandOption("-p|--pattern")]
            public string? SearchPattern { get; init; }

            [Description("Filename to search.")]
            [CommandOption("-f|--file")]
            public string? FileName { get; init; }            
        }
        public override int Execute([NotNull]CommandContext context, [NotNull] Settings settings)
        {
            throw new NotImplementedException();
        }
    }
}
