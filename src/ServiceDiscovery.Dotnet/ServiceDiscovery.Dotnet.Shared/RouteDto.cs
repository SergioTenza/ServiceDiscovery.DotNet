namespace ServiceDiscovery.Dotnet.Shared;

public record RouteDto
{
    public required string RouteId {get;init;}
    public required string ClusterId {get;init;}
    public required string MatchPath {get;init;}
}
