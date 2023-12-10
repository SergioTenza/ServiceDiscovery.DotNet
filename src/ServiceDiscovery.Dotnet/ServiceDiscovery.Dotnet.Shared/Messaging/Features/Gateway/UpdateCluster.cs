namespace ServiceDiscovery.Dotnet.Shared;

public record UpdateCluster
{
    public required ClusterDto Cluster {get;init;}
}
