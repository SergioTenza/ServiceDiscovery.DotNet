namespace ServiceDiscovery.Dotnet.Shared;

public record ReverseProxy
{
    public IEnumerable<RouteDto> Routes {get;init;} = [];
    public IEnumerable<ClusterDto> Clusters {get;init;} = [];
}
