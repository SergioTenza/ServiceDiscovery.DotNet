namespace ServiceDiscovery.Dotnet.Shared;

public class Cluster : DomainEntity
{
    public string ClusterId { get; set; } = string.Empty;
    public SessionAffinityDto SessionAffinity { get; set; } = new();
    public Dictionary<string, DestinationConfigDto> Destinations { get; set; } = new Dictionary<string, DestinationConfigDto>(StringComparer.OrdinalIgnoreCase);
}
