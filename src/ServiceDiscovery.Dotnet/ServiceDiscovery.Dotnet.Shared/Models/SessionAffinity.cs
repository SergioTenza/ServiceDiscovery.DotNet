namespace ServiceDiscovery.Dotnet.Shared;

public class SessionAffinity : DomainEntity
{
    public bool Enabled {get;set;} = true;
    public string Policy {get;set;} = "Cookie";
    public string AffinityKeyName {get;set;} = ".Yarp.ReverseProxy.Affinity";
}
