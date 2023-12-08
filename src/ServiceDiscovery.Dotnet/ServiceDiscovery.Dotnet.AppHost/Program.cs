var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.ServiceDiscovery_Dotnet_ApiService>("apiservice");

builder.AddProject<Projects.ServiceDiscovery_Dotnet_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.AddProject<Projects.ServiceDiscovery_Dotnet_ApiGateway>("servicediscovery.dotnet.apigateway");

builder.Build().Run();
