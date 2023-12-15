using Fluxor;

namespace ServiceDiscovery.Dotnet.Shared;

[FeatureState]
public record ClustersState
{
    public bool IsLoading {get;init;}
    public IEnumerable<ClusterDto> Clusters {get;init;}= [];
    private ClustersState() { }    
}
