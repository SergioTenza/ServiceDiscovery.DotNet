namespace ServiceDiscovery.Dotnet.Shared;

public record ClusterDto
{
    public required string ClusterId {get;init;}
    public required string Destination {get;init;}
    public required string Path {get;init;}
}
