namespace ServiceDiscovery.Dotnet.Shared;

public class MatchDto
{
    public string Path {get;set;} = string.Empty;
    public IEnumerable<string> Hosts {get; set;} = [];
}
