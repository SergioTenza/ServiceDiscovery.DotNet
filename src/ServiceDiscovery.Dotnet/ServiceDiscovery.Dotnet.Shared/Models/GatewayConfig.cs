namespace ServiceDiscovery.Dotnet.Shared;

public record GatewayConfig
{
    public IEnumerable<RouteDto> Routes {get;init;} = [];
    public IEnumerable<ClusterDto> Clusters {get;init;} = [];
}
