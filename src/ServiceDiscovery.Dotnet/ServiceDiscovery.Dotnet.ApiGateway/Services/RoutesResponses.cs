using System.Collections.Immutable;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return Results.Problem(ex.Message, nameof(routeDto), 500, "Error Adding the route.");
        }
    };
    public static Func<RouteDto, RouteConfig,InMemoryConfigProvider, IResult> UpdateRoute = (routeDto, routeConfig,configProvider) =>
    {
        try
        {
            var routes = configProvider.GetConfig().Routes.ToList();
            var clusters = configProvider.GetConfig().Clusters.ToImmutableList();
            return GenerateResultFromRoutes(configProvider, clusters, routes, routeDto, routeConfig);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, nameof(routeDto), 500, "Error Adding the route.");
        }
    };

     public static Func<RouteConfig,InMemoryConfigProvider, IResult> DeleteRoute = (routeConfig,configProvider) =>
    {
        try
        { 
            var routes = configProvider.GetConfig().Routes.Where(r=> r.RouteId != routeConfig.RouteId).ToArray();
            var clusters = configProvider.GetConfig().Clusters.Where(c=> c.ClusterId != routeConfig.ClusterId).ToArray();
            configProvider.Update(routes,clusters);
            return Results.Ok(routeConfig.RouteId);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, nameof(routeConfig), 500, "Error Adding the route.");
        }
    };

    
    private static IResult GenerateResultFromRoutes(InMemoryConfigProvider configProvider, IEnumerable<ClusterConfig> clusters, IEnumerable<RouteConfig> routes, RouteDto routeDto, RouteConfig routeConfig)
    {
        try
        {
            var updatedRoute = new RouteConfig
            {
                RouteId = routeDto.RouteId,
                ClusterId = routeDto.ClusterId,
                Match = routeDto.Match?.ToRouteMatch() ?? new RouteMatch()
            };
            var updatedRoutes = routes.Where(r => r.RouteId != routeConfig.RouteId).ToList();
            updatedRoutes.Add(updatedRoute);
            configProvider.Update(updatedRoutes, clusters.ToList());
            return Results.Ok(routeDto);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message, nameof(routeDto), 500, "Error Updating the route.");
        }

    }
}
