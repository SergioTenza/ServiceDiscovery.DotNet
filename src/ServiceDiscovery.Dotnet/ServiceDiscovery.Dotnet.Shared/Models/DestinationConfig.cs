namespace ServiceDiscovery.Dotnet.Shared;

public class DestinationConfig : DomainEntity
{
    public string Address {get;set;} = string.Empty;
    public Dictionary<string, string> Metadata {get;set;} = new Dictionary<string, string>
    {
        { "debug", "true" }
    };
}
