namespace ServiceDiscovery.Dotnet.Shared;

public class RouteDto : DomainEntity
{
    public string RouteId {get;set;} = string.Empty;
    public string ClusterId {get;set;} = string.Empty;
    public MatchDto? Match {get;set;}
    public bool Selected {get;set;}
}
