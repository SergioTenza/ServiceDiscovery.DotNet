var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.ServiceDiscovery.Dotnet_ApiService>("apiservice");

builder.AddProject<Projects.ServiceDiscovery.Dotnet_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
