namespace ServiceDiscovery.Dotnet.Shared;

public class Match : DomainEntity
{
    public string Path {get;set;} = string.Empty;
    public IEnumerable<string> Hosts {get; set;} = [];
}
