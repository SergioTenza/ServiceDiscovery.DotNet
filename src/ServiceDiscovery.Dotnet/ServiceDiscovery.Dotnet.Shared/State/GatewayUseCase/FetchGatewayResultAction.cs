namespace ServiceDiscovery.Dotnet.Shared;

public record FetchGatewayResultAction(IEnumerable<RouteDto> Routes,IEnumerable<ClusterDto> Clusters);
