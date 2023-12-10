using System.Collections.Immutable;
using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public class UpdateGatewayConsumer : IConsumer<UpdateGateway>
{
    private readonly InMemoryConfigProvider _inMemoryConfigProvider;
    private readonly string _rediConnectionString;
    public UpdateGatewayConsumer(InMemoryConfigProvider inMemoryConfigProvider,IConfiguration configuration)
    {
        this._inMemoryConfigProvider = inMemoryConfigProvider;
        _rediConnectionString = configuration.GetConnectionString("cache") ?? 
            throw new InvalidOperationException("Connection string for redis is mandatory");
    }

    public async Task Consume(ConsumeContext<UpdateGateway> context)
    {
        var redis = ConnectionMultiplexer.Connect(_rediConnectionString);        
        var server = redis.GetServer(redis.GetEndPoints().First());
        var routes = server.Keys(database: 0, pattern: "Routes:*");
        var clusters = server.Keys(database: 0, pattern: "Clusters:*");
        var routeConfigs = routes.Select(r => RedisOperations.GetRouteConfigFromValue(redis.GetDatabase().StringGet(r)))
                                .Where(c => c is not null)
                                .ToImmutableList() ?? [];
        var clusterConfigs = clusters.Select(c => RedisOperations.GetClusterConfigFromValue(redis.GetDatabase().StringGet(c)))
                                .Where(c => c is not null)
                                .ToImmutableList() ?? [];
        _inMemoryConfigProvider.Update(routeConfigs!,clusterConfigs!);
        await Task.CompletedTask;
    }
}
