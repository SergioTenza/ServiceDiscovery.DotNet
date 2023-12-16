using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public class AddRouteConsumer : IConsumer<AddRoute>
{
    private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
    public AddRouteConsumer(InMemoryConfigProvider inMemoryConfigProvider)
    {
 
        _inMemoryConfigProvider = inMemoryConfigProvider;            
    }
    public async Task Consume(ConsumeContext<AddRoute> context)
    {
 
        await Task.CompletedTask;
    }
}
