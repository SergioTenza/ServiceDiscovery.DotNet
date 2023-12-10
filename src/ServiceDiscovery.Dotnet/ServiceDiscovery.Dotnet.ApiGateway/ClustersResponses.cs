using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class ClustersResponses
{
    // public static Func<ClusterDto, List<RouteConfig>, List<ClusterConfig>, InMemoryConfigProvider, IResult> InsertCluster = (clusterDto, routes, clusters, configProvider) =>
    //     {
    //         clusters.Add(clusterDto.ToRouteConfig());
    //         configProvider.Update(routes, clusters);
    //         return Results.Accepted($"http://localhost:5024/v1/routes/{routeDto.RouteId}", routeDto);
    //     };
}
