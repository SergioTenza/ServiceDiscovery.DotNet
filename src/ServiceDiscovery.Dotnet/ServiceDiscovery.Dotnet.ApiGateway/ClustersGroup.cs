using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class ClustersGroup
{
    public static RouteGroupBuilder Clusters(this RouteGroupBuilder builder)
    {
        builder.MapGet("/clusters", () => Array.Empty<ClusterConfig>());

        builder.MapGet("/clusters/{clusterId}", (string clusterId) =>
            new List<RouteConfig>().Where(r => r.ClusterId == clusterId) switch
            {
                IEnumerable<ClusterConfig> clusters => Results.Ok(),
                null => Results.NotFound(),
                _ => Results.NotFound()
            }
        );
        // builder.MapPost("/clusters", (
        //     ClusterDto clusterDto,
        //     List<RouteConfig> routes,
        //     List<ClusterConfig> clusters,
        //     InMemoryConfigProvider configProvider) =>
        //         clusters.Any(r => r.ClusterId == clusterDto.ClusterId) switch
        //         {
        //             true => Results.Conflict(),
        //             false => ClustersResponses.InsertCluster(clusterDto, routes, clusters, configProvider)
        //         });
        // builder.MapPut("/clusters/{clusterId}", (string clusterId, List<ClusterConfig> cluster) =>
        //     cluster.Where(r => r.ClusterId == clusterId) switch
        //     {
        //         IEnumerable<ClusterConfig> clusters => [],
        //         null => Results.NotFound()
        //     }
        // );
        return builder;
    }
}
