using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;
[FeatureState]
public record GatewayState
{
    public bool IsLoading {get;init;}
    public IEnumerable<RouteDto> Routes {get;init;}= [];
    public IEnumerable<ClusterDto> Clusters {get;init;}= [];
    private GatewayState() { }    
}
