using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

[FeatureState]
public record RoutesState
{    
    public bool IsLoading {get;init;}
    public IEnumerable<RouteDto> Routes {get;init;}= [];
    private RoutesState() { }    
}