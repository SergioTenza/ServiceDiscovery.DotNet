namespace ServiceDiscovery.Dotnet.Shared;

public record RouteDto
{
    public string RouteId {get;set;}
    public string ClusterId {get;set;}
    public string MatchPath {get;set;}
    public bool Selected {get;set;}
}
