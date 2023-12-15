using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class ClustersGroup
{
    public static RouteGroupBuilder Clusters(this RouteGroupBuilder builder)
    {
        builder.MapGet("/clusters",  ([FromServices]InMemoryConfigProvider configProvider) =>
            Results.Ok(configProvider.GetConfig().Clusters.Select(r => r.ToClusterDto()).ToArray()));

        builder.MapGet("/clusters/{clusterId}", (string clusterId,[FromServices] InMemoryConfigProvider configProvider) =>
            configProvider.GetConfig().Clusters.Where(r => r.ClusterId == clusterId).FirstOrDefault() switch
            {
                ClusterConfig cluster => Results.Ok(cluster.ToClusterDto()),
                null => Results.NotFound()
            }
        );
        builder.MapPost("/clusters", (
            ClusterDto clusterDto,
            [FromServices] InMemoryConfigProvider configProvider) =>
                configProvider.GetConfig().Clusters.Any(r => r.ClusterId == clusterDto.ClusterId) switch
                {
                    true => Results.Conflict(),
                    false => ClustersResponses.InsertCluster(clusterDto, configProvider)
                });
        builder.MapPut("/clusters/{clusterId}", (string clusterId, [FromServices] InMemoryConfigProvider configProvider) =>
            configProvider.GetConfig().Clusters.Where(r => r.ClusterId == clusterId).FirstOrDefault() switch
            {
                ClusterConfig cluster => Results.Ok(cluster),
                null => Results.NotFound()
            }
        );
        return builder;
    }
}
