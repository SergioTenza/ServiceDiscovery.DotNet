namespace ServiceDiscovery.Dotnet.Shared;

public record ClusterDto
{
    public string ClusterId { get; set; } = string.Empty;
    public SessionAffinityDto sessionAffinity { get; set; } = new();
    public Dictionary<string, DestinationConfigDto> Destination { get; set; } = new Dictionary<string, DestinationConfigDto>(StringComparer.OrdinalIgnoreCase);
}
