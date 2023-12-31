﻿namespace ServiceDiscovery.Dotnet.Shared;

public record RouteDto
{
    public string RouteId {get;set;} = string.Empty;
    public string ClusterId {get;set;} = string.Empty;
    public MatchDto Match {get;set;} = new MatchDto();
    public bool Selected {get;set;}
}
