namespace ServiceDiscovery.Dotnet.Shared;

public record AddCluster
{
    public required ClusterDto Cluster {get;init;}

}
