namespace ServiceDiscovery.Dotnet.Shared;

public record MatchDto
{
    public string Path {get;init;} = string.Empty;
    public IEnumerable<string> Hosts {get; init;} = [];
}
