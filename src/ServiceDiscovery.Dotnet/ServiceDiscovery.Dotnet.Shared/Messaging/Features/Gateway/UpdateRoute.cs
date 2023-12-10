namespace ServiceDiscovery.Dotnet.Shared;

public record UpdateRoute
{
    public required RouteDto Cluster {get;init;}
}
