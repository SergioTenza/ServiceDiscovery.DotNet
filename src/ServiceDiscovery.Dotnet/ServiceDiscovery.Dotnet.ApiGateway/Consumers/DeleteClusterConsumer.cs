using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;


namespace ServiceDiscovery.Dotnet.ApiGateway;

public class DeleteClusterConsumer : IConsumer<DeleteCluster>
{
   //private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
   // public DeleteClusterConsumer(InMemoryConfigProvider inMemoryConfigProvider)
   // {
 
   //     _inMemoryConfigProvider = inMemoryConfigProvider;            
   // }
    public async Task Consume(ConsumeContext<DeleteCluster> context)
    {
 
        await Task.CompletedTask.ConfigureAwait(true);
    }
}
