﻿using System.Collections.Immutable;
using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public class UpdateGatewayConsumer : IConsumer<UpdateGateway>
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
    public UpdateGatewayConsumer(ConnectionMultiplexer connectionMultiplexer,InMemoryConfigProvider inMemoryConfigProvider)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _inMemoryConfigProvider = inMemoryConfigProvider;            
    }

    public async Task Consume(ConsumeContext<UpdateGateway> context)
    {
        var (routes, clusters) = _connectionMultiplexer.GetProxyFromRedis(0);
        _inMemoryConfigProvider.Update(routes,clusters);
        await Task.CompletedTask;
    }
}