var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");
var queue = builder.AddRabbitMQ("rabbitmq");
var postgres = builder.AddPostgres("postgresdb")
						.AddDatabase("AppDb");

var apiservice = builder.AddProject<Projects.ServiceDiscovery_Dotnet_ApiService>("apiservice");

builder.AddProject<Projects.ServiceDiscovery_Dotnet_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.AddProject<Projects.ServiceDiscovery_Dotnet_ApiGateway>("apigateway")
	.WithReference(cache)
	.WithReference(queue)
	.WithReference(postgres);

builder.Build().Run();
