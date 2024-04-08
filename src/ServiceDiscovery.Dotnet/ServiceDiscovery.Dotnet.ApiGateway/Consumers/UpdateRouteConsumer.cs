using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;


namespace ServiceDiscovery.Dotnet.ApiGateway;

public class UpdateRouteConsumer : IConsumer<UpdateRoute>
{
    
    //private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
    //public UpdateRouteConsumer(InMemoryConfigProvider inMemoryConfigProvider)
    //{
 
    //    _inMemoryConfigProvider = inMemoryConfigProvider;            
    //}
    public async Task Consume(ConsumeContext<UpdateRoute> context)
    {
 
        await Task.CompletedTask.ConfigureAwait(true);
    }
}
