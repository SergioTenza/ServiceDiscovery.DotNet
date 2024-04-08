using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;


namespace ServiceDiscovery.Dotnet.ApiGateway;

public class DeleteRouteConsumer : IConsumer<DeleteRoute>
{
   //private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
   // public DeleteRouteConsumer(InMemoryConfigProvider inMemoryConfigProvider)
   // {
 
   //     _inMemoryConfigProvider = inMemoryConfigProvider;            
   // }
    public async Task Consume(ConsumeContext<DeleteRoute> context)
    {
 
        await Task.CompletedTask.ConfigureAwait(true);
    }
}
