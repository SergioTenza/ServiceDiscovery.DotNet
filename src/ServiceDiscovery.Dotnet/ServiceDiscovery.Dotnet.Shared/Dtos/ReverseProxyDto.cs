namespace ServiceDiscovery.Dotnet.Shared;

public record ReverseProxyDto
{
    public IEnumerable<RouteDto> Routes {get;init;} = [];
    public IEnumerable<ClusterDto> Clusters {get;init;} = [];
}
