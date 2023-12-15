using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class ClustersResponses
{
    public static Func<ClusterDto, InMemoryConfigProvider, IResult> InsertCluster = (clusterDto, configProvider) =>
        {
            try
            {
                var clusters = configProvider.GetConfig().Clusters.ToList();
                var routes = configProvider.GetConfig().Routes;
                clusters.Add(clusterDto.ToClusterConfig());
                configProvider.Update(routes, clusters);
                return Results.Ok(clusterDto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, nameof(clusterDto), 500, "Error Adding the cluster.");
            }

        };
}
