namespace ServiceDiscovery.Dotnet.Shared;

public class ClusterDto
{
    public string ClusterId { get; set; } = string.Empty;
    public SessionAffinityDto SessionAffinity { get; set; } = new();
    public Dictionary<string, DestinationConfigDto> Destination { get; set; } = new Dictionary<string, DestinationConfigDto>(StringComparer.OrdinalIgnoreCase);
}
