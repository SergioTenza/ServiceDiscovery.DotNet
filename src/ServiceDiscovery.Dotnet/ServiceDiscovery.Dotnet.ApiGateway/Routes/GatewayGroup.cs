using Microsoft.AspNetCore.Http.HttpResults;
using ServiceDiscovery.Dotnet.Shared;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class GatewayGroup
{
    public static RouteGroupBuilder Gateway(this RouteGroupBuilder builder)
    {
        builder.MapGet("/gateway",(InMemoryConfigProvider provider)=>
        {   
            var config = provider.GetConfig();  
            return Results.Ok(new GatewayConfig
            {
              
              Routes = config.Routes.Select(r=> r.ToRouteDto()),
              Clusters = config.Clusters.Select(c=> c.ToClusterDto())  
            });
        
        });
        return builder;
    }
}
