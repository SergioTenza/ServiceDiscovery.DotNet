﻿using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class RoutesGroup
{
    public static RouteGroupBuilder Routes(this RouteGroupBuilder builder)
    {
        builder.MapGet("/routes", ([FromServices]InMemoryConfigProvider configProvider) =>
            Results.Ok(configProvider.GetConfig().Routes.Select(r => r.ToRouteDto()).ToArray()));

        builder.MapGet("/routes/{routeId}", (string routeId, [FromServices]InMemoryConfigProvider configProvider) =>
            configProvider.GetConfig().Routes.Where(r => r.RouteId == routeId).FirstOrDefault() switch
            {
                RouteConfig route => Results.Ok( route.ToRouteDto()),
                null => Results.NotFound()
            }
        );
        builder.MapPost("/routes", (
            [FromBody]RouteDto routeDto,
            [FromServices]InMemoryConfigProvider configProvider) =>
                configProvider.GetConfig().Routes.Any(r => r.RouteId == routeDto.RouteId) switch
                {
                    true => Results.Conflict(),
                    false => RoutesResponses.InsertRoute(routeDto, configProvider)
                });
        builder.MapPut("/routes/{routeId}", (RouteDto routeDto,[FromServices]InMemoryConfigProvider configProvider) =>
            configProvider.GetConfig().Routes.Where(r => r.RouteId == routeDto.RouteId).FirstOrDefault() switch
            {
                RouteConfig routeConfig => RoutesResponses.UpdateRoute(routeDto, routeConfig,configProvider),
                null => Results.NotFound()
            }
        );
        builder.MapDelete("/routes/{routeId}", (string routeId,[FromServices]InMemoryConfigProvider configProvider) =>
        {
            var exists = configProvider.GetConfig().Routes.Where(r => r.RouteId == routeId).FirstOrDefault();
            return exists switch 
            {
                RouteConfig config => RoutesResponses.DeleteRoute(config,configProvider),
                null => Results.NotFound()
            };
           
        });
        return builder;
    }
}
//return Results.Accepted($"http://localhost:5024/v1/routes/{routeDto.RouteId}",routeDto);       