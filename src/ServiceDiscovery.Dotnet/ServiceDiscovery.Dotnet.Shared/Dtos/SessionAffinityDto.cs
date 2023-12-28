namespace ServiceDiscovery.Dotnet.Shared;

public record SessionAffinityDto
{
    public bool Enabled {get;set;} = true;
    public string Policy {get;set;} = "Cookie";
    public string AffinityKeyName {get;set;} = ".Yarp.ReverseProxy.Affinity";
}
