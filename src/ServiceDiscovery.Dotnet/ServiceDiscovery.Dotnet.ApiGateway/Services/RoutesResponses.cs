using System.Collections.Immutable;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class RoutesResponses
{
    public static Func<RouteDto, InMemoryConfigProvider, IResult> InsertRoute = (routeDto, configProvider) =>
    {
        try
        {
            var routes = configProvider.GetConfig().Routes.ToList();
            var clusters = configProvider.GetConfig().Clusters.ToImmutableList();
            routes.Add(routeDto.ToRouteConfig());
            configProvider.Update(routes, clusters);
            return Results.Ok(routeDto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message,nameof(routeDto),500,"Error Adding the route.");
        }
    };
}
