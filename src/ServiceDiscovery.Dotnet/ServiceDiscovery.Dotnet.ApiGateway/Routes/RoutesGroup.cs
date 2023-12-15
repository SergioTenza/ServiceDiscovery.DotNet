using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class RoutesGroup
{
    public static RouteGroupBuilder Routes(this RouteGroupBuilder builder)
    {
        builder.MapGet("/routes", ([FromServices]List<RouteConfig> routes) =>
            routes.ToArray());

        // builder.MapGet("/routes/{routeId}", (string routeId, List<RouteConfig> routes) =>
        //     routes.Where(r => r.RouteId == routeId).FirstOrDefault() switch
        //     {
        //         RouteConfig route => Results.Ok(new RouteDto
        //         {
        //             RouteId = route.RouteId,
        //             ClusterId = route.ClusterId ?? string.Empty,
        //             MatchPath = route.Match.Path ?? string.Empty
        //         }),
        //         null => Results.NotFound()
        //     }
        // );
        builder.MapPost("/routes", (
            [FromBody]RouteDto routeDto,
            [FromServices]List<RouteConfig> routes,
            [FromServices]List<ClusterConfig> clusters,
            [FromServices]InMemoryConfigProvider configProvider) =>
                routes.Any(r => r.RouteId == routeDto.RouteId) switch
                {
                    true => Results.Conflict(),
                    false => RoutesResponses.InsertRoute(routeDto, routes, clusters, configProvider)
                });
        // builder.MapPut("/routes/{routeId}", (string routeId, List<RouteConfig> routes) =>
        //     routes.Where(r => r.RouteId == routeId).FirstOrDefault() switch
        //     {
        //         RouteConfig route => Results.Ok(new RouteDto
        //         {
        //             RouteId = route.RouteId,
        //             ClusterId = route.ClusterId ?? string.Empty,
        //             MatchPath = route.Match.Path ?? string.Empty
        //         }),
        //         null => Results.NotFound()
        //     }
        // );
        return builder;
    }
}
