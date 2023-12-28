namespace ServiceDiscovery.Dotnet.Shared;

public record MatchDto
{
    public string Path {get;set;} = string.Empty;
    public IEnumerable<string> Hosts {get; set;} = [];
}
