using System.Collections.Immutable;
using MassTransit;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public class UpdateGatewayConsumer : IConsumer<UpdateGateway>
{
   //private readonly InMemoryConfigProvider _inMemoryConfigProvider;  
    
   // public UpdateGatewayConsumer(InMemoryConfigProvider inMemoryConfigProvider)
   // {
 
   //     _inMemoryConfigProvider = inMemoryConfigProvider;            
   // }
    public async Task Consume(ConsumeContext<UpdateGateway> context)
    {
 
        await Task.CompletedTask.ConfigureAwait(true);
    }
}
