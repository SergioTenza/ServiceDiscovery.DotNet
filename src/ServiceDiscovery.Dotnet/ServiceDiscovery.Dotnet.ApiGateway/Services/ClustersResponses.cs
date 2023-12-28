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

        public static Func<ClusterDto, InMemoryConfigProvider, IResult> UpdateCluster = (clusterDto, configProvider) =>
        {
            try
            {
                var clusters = configProvider.GetConfig().Clusters.ToList();
                var routes = configProvider.GetConfig().Routes;

                var updateCluster = clusters.Where(c=> c.ClusterId == clusterDto.ClusterId).FirstOrDefault();
                if(updateCluster is not null && updateCluster.Destinations is not null)
                {
                    Dictionary<string,Shared.DestinationConfig> destinations = [];
                    foreach (var key in clusterDto.Destinations.Keys)
                    {
                       if(updateCluster.Destinations.TryGetValue(key,out var existingConfig))
                       {
                            var updatedConfig = new Shared.DestinationConfig
                            {

                            };
                       }//.Append(key, clusterDto.Destinations[key]);                        
                    }

                    var clustersWitouhtUpdated = clusters.Where(c => c.ClusterId != updateCluster.ClusterId).ToList();
                    var updatedCluster = new ClusterConfig
                    {
                        ClusterId = clusterDto.ClusterId,
                        SessionAffinity = clusterDto.SessionAffinity.ToSessionAffinityConfig(),
                        Destinations = (IReadOnlyDictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>)destinations
                    };
                    clustersWitouhtUpdated.Add(updatedCluster);
                    configProvider.Update(routes, clustersWitouhtUpdated);
                    return Results.Ok(clusterDto);
                }
                configProvider.Update(routes, clusters);
                return Results.Ok(clusterDto);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, nameof(clusterDto), 500, "Error Adding the cluster.");
            }

        };
}
