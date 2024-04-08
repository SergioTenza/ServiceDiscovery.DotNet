using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public class AddClusterConsumer : IConsumer<AddCluster>
{
    //private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
    //public AddClusterConsumer(InMemoryConfigProvider inMemoryConfigProvider)
    //{
    //    _inMemoryConfigProvider = inMemoryConfigProvider;            
    //}
    public async Task Consume(ConsumeContext<AddCluster> context)
    {
        await Task.CompletedTask.ConfigureAwait(true);
    }
}
