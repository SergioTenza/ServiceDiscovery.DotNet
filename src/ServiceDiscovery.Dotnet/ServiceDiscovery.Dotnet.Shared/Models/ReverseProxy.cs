namespace ServiceDiscovery.Dotnet.Shared;

public class ReverseProxy : DomainEntity
{
    public IEnumerable<RouteDto> Routes {get;init;} = [];
    public IEnumerable<ClusterDto> Clusters {get;init;} = [];
}
