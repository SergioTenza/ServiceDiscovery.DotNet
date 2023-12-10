namespace ServiceDiscovery.Dotnet.Shared;

public record AddRoute
{
    public required RouteDto Cluster {get;init;}
}
