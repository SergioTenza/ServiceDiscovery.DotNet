using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class RoutesResponses
{
    public static Func<RouteDto, List<RouteConfig>, List<ClusterConfig>,InMemoryConfigProvider,IResult> InsertRoute = (routeDto, routes,clusters,configProvider) =>
    {          
        routes.Add(routeDto.ToRouteConfig());
        configProvider.Update(routes,clusters);
        return Results.Accepted($"http://localhost:5024/v1/routes/{routeDto.RouteId}",routeDto);       
    };
}
