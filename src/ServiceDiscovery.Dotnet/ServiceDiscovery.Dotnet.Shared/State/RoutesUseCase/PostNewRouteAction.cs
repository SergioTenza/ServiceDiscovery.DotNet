namespace ServiceDiscovery.Dotnet.Shared;

public record PostNewRouteAction
{
    public required RouteDto Dto {get;init;}
}
