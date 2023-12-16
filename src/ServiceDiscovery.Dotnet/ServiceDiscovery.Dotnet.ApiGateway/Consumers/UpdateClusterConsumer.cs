using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;


namespace ServiceDiscovery.Dotnet.ApiGateway;

public class UpdateClusterConsumer : IConsumer<UpdateCluster>
{
    private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
    public UpdateClusterConsumer(InMemoryConfigProvider inMemoryConfigProvider)
    {
 
        _inMemoryConfigProvider = inMemoryConfigProvider;            
    }
    public async Task Consume(ConsumeContext<UpdateCluster> context)
    {
 
        await Task.CompletedTask;
    }
}
